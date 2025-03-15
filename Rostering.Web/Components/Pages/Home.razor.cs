using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Rostering.Application;
using Rostering.Application.Models;
using Rostering.Domain;
using VYaml.Serialization;

namespace Rostering.Web.Components.Pages;

public partial class Home
{
    private bool ProcessReady { get; set; } = false;
    private List<Tuple<int, List<Shift>>> _possibleSchedules = [];

    public async Task FileUploaded(InputFileChangeEventArgs @event)
    {
        try
        {
            var fileUploaded = @event.File;
            var fileStream = fileUploaded.OpenReadStream();
            var rosteringData = await YamlSerializer.DeserializeAsync<RosteringConfiguration>(fileStream);

            var rosterService = new RosterService();

            _possibleSchedules = rosterService.RosterDoctors(rosteringData);

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
        var contenido = "";
        foreach (var (generation, schedule) in _possibleSchedules)
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
}