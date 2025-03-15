namespace Rostering.Domain;

public class Request
{
    public List<DateTime>? DaysOff { get; set; }
    public List<DayOfWeek>? DaysOfWeekOff { get; set; }
    public Vacations? VacationsDays { get; set; }

    public record Vacations(DateTime Start, DateTime End);
}

public class Doctor
{
    public string Name { get; set; } = default!;
    public Request? Requests { get; set; }

    private readonly int _maxAssignedDays = 4;
    private List<DateTime> _shiftsDates = new();

    public bool IsDoctorAvailableForShift(Shift shift)
    {
        if(shift.AssignedDoctors.Contains(this))
            return false;
        
        if (_shiftsDates.Count >= _maxAssignedDays)
            return false;

        if (HasAssignedDaysTheDaysBefore(shift.Date))
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

    public void AssignShiftDate(Shift shift) => _shiftsDates.Add(shift.Date);

    public void ResetAssignedDates() => _shiftsDates.Clear();

    private bool HasAssignedDaysTheDaysBefore(DateTime date)
    {
        var previousShift = _shiftsDates.Where(x => x < date)
                                  .OrderByDescending(s => s.Date)
                                  .FirstOrDefault();
        
        return previousShift != default && (previousShift.Date == date.AddDays(-1) || previousShift.Date == date.AddDays(-2));
    }
    
}