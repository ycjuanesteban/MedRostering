using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Rostering.Domain;
using Rostering.Web.Models;
using VYaml.Serialization;

namespace Rostering.Web.Components.Pages;

public partial class Home
{
    private bool ProcessReady { get; set; } = false;
    private List<Tuple<int, List<Shift>>> possibleSchedules = new List<Tuple<int, List<Shift>>>();

    public async Task FileUploaded(InputFileChangeEventArgs @event)
    {
        try
        {
            var fileUploaded = @event.File;
            var fileStream = fileUploaded.OpenReadStream();
            var rosteringData = await YamlSerializer.DeserializeAsync<RosteringConfiguration>(fileStream);

            var scheduler =
                new ShiftScheduler(GetDoctors(rosteringData!), rosteringData!.Month,
                    GetDaysWithTwoDoctors(rosteringData));

            for (var i = 0; i < rosteringData.ShiftQuantity; i++)
            {
                possibleSchedules.Add(new Tuple<int, List<Shift>>(i, scheduler.AssignShifts()));
            }

            ProcessReady = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task DownloadData()
    {
        string contenido = "";
        foreach ((var generation, var schedule) in possibleSchedules)
        {
            contenido += $"Guardia {generation} \n";
            foreach (var shift in schedule)
            {
                contenido += $"\n{shift.Date,-8:dd/MM/yy} -- {shift.Date.DayOfWeek.DayOfWeekToSpanish(),-9} " +
                             $"Doctores: {string.Join(",", shift.AssignedDoctors.Select(x => x.Name).ToArray())}";
            }

            var groupedDoctors = schedule
                .SelectMany(x => x.AssignedDoctors)
                .GroupBy(x => x.Name)
                .Select(x => new { Doctor = x.Key, Count = x.Count() }).ToList();

            contenido += "\n";
            foreach (var group in groupedDoctors)
            {
                contenido += $"\n{group.Doctor, -10} {group.Count} guardias";
            }
            contenido += "\n*****************************************************\n\n\n";
        }

        var archivoBytes = System.Text.Encoding.UTF8.GetBytes(contenido);
        await js.InvokeVoidAsync("downloadFileFromStream", "Guardias.txt", archivoBytes);
    }

    private List<Doctor> GetDoctors(RosteringConfiguration rosteringData)
    {
        var firstDayOfMonth = rosteringData.Month.ConvertToFirstDayOfMonth();

        return rosteringData!.Doctors.Select(d => new Doctor
        {
            Name = d.Name,
            Requests = d.Requests == null
                ? null
                : new Request
                {
                    DaysOff = d.Requests.DaysOff?.Count == 0
                        ? null
                        : d.Requests.DaysOff?.Select(df => df.ConvertToDate(firstDayOfMonth)).ToList(),
                    DaysOfWeekOff = d.Requests.DaysOfWeekOff?.Count == 0
                        ? null
                        : d.Requests.DaysOfWeekOff?.Select(x => x.SpanishDayToDayOfWeek()).ToList(),
                    VacationsDays = d.Requests.Vacations == null || d.Requests.Vacations.Start == 0
                        ? null
                        : new Request.Vacations(d.Requests.Vacations.Start.ConvertToDate(firstDayOfMonth),
                            d.Requests.Vacations.End.ConvertToDate(firstDayOfMonth))
                },
            AssignedDays = 0
        }).ToList();
    }

    private List<DayOfWeek> GetDaysWithTwoDoctors(RosteringConfiguration rosteringData) =>
        rosteringData.DaysWithTwoDoctors.Select(x => x.SpanishDayToDayOfWeek()).ToList();
}