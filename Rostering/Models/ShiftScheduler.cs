namespace Rostering.Models;

public class Shift
{
    public DateTime Date { get; set; }
    public List<Doctor> AssignedDoctors { get; set; } = new();
}

public class ShiftScheduler
{
    private readonly List<Doctor> _doctors;
    private List<Shift> Shifts { get; set; }
    private List<DayOfWeek>? DaysWithTwoDoctors { get; set; }
    private readonly string[] _months = ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];

    public ShiftScheduler(List<Doctor> doctors, string month, List<DayOfWeek> daysWithTwoDoctors)
    {
        _doctors = doctors;
        Shifts = GenerateShifts(month);
        DaysWithTwoDoctors = daysWithTwoDoctors;
    }

    public List<Shift> AssignShifts()
    {
        foreach (var shift in Shifts)
        {
            if (DaysWithTwoDoctors != null && DaysWithTwoDoctors.Contains(shift.Date.DayOfWeek))
            {
                FindDoctor(shift);
            }
            FindDoctor(shift);
        }
        
        return Shifts;
    }

    private void FindDoctor(Shift shift)
    {
        var availableDoctor = FindAvailableDoctor(shift);
        if (availableDoctor == null) return;
        
        shift.AssignedDoctors.Add(availableDoctor);
        availableDoctor.AssignedDays++;
    }
    
    private List<Shift> GenerateShifts(string month)
    {
        var monthNumber = Array.IndexOf(_months, month.ToLower()) + 1;

        var firstDayOfMonth = new DateTime(DateTime.Now.Year, monthNumber, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var shifts = new List<Shift>();
        for (var date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
        {
            shifts.Add(new Shift { Date = date });
        }
        return shifts;
    }

    private Doctor? FindAvailableDoctor(Shift shift)
    {
        return Shuffle(_doctors).FirstOrDefault(d => d.IsDoctorAvailableForShift(Shifts, shift))!;

        // shuffle the doctors to avoid the same orden each time 
        IList<T> Shuffle<T>(IList<T> list)
        {
            Random rng = new();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }

            return list;
        }
    }
}
