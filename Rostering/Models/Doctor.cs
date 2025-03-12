namespace Rostering.Models;

public class Request
{
    public List<DateTime>? DaysOff { get; set; }
    public List<DayOfWeek>? DaysOfWeekOff { get; set; }
    public Vacations? VacationsDays { get; set; }
    
    public record Vacations(DateTime Start, DateTime End);
}

public class Doctor
{
    public string Name { get; set; }
    public Request? Requests { get; set; }
    public int AssignedDays { get; set; }

    public bool IsDoctorAvailableForShift(List<Shift> shifts, Shift shift)
    {
        if(shift.AssignedDoctors.Contains(this))
            return false;
        
        if (AssignedDays >= 3)
            return false;

        if (HasAssignedDaysTheDaysBefore(shifts, shift.Date))
            return false;

        switch (Requests)
        {
            case { DaysOff: not null } when Requests.DaysOff.Any(r => r == shift.Date):
            case { DaysOfWeekOff: not null } when Requests.DaysOfWeekOff.Any(r => r == shift.Date.DayOfWeek):
            case { VacationsDays: not null } when Requests.VacationsDays.Start <= shift.Date && Requests.VacationsDays.End >= shift.Date:    
                return false;
            default:
                return true;
        }
    }

    private bool HasAssignedDaysTheDaysBefore(List<Shift> shifts, DateTime date)
    {
        var previousShift = shifts.Where(s => s.AssignedDoctors.Contains(this) && s.Date < date)
                                  .OrderByDescending(s => s.Date)
                                  .FirstOrDefault();
        return previousShift != null && (previousShift.Date == date.AddDays(-1) || previousShift.Date == date.AddDays(-2));
    }
    
}