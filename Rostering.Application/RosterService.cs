using Rostering.Application.Models;
using Rostering.Domain;

namespace Rostering.Application;

public class RosterService
{
    private readonly List<Tuple<int, List<Shift>>> _possibleSchedules = [];
    
    public List<Tuple<int, List<Shift>>> RosterDoctors(RosteringConfiguration rosteringData)
    {
        var scheduler =
            new ShiftScheduler(GetDoctors(rosteringData), rosteringData.Month.ConvertToFirstDayOfMonth(), GetDaysWithTwoDoctors(rosteringData));
        
        for (var i = 0; i < rosteringData.ShiftQuantity; i++)
        {
            _possibleSchedules.Add(new Tuple<int, List<Shift>>(i, scheduler.AssignShifts()));
        }

        return _possibleSchedules;
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
                    DaysOff = d.Requests.DaysOff?.Count == 0 ? null : d.Requests.DaysOff?.Select(df => df.ConvertToDate(firstDayOfMonth)).ToList(),
                    DaysOfWeekOff = d.Requests.DaysOfWeekOff?.Count == 0 ? null : d.Requests.DaysOfWeekOff?.Select(x => x.SpanishDayToDayOfWeek()).ToList(),
                    VacationsDays = d.Requests.Vacations == null || d.Requests.Vacations.Start == 0
                        ? null
                        : new Request.Vacations(d.Requests.Vacations.Start.ConvertToDate(firstDayOfMonth),
                            d.Requests.Vacations.End.ConvertToDate(firstDayOfMonth))
                }
        }).ToList();
    }

    private List<DayOfWeek> GetDaysWithTwoDoctors(RosteringConfiguration rosteringData) =>
        rosteringData.DaysWithTwoDoctors.Select(x => x.SpanishDayToDayOfWeek()).ToList();
}