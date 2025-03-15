namespace Rostering.Domain;

public class Shift
{
    private readonly List<Doctor> _assignedDoctors = [];
    
    public DateTime Date { get; init; }
    public bool RequireTwoDoctors { get; init; }
    
    public IReadOnlyList<Doctor> AssignedDoctors => _assignedDoctors;

    public void AssignDoctor(Doctor doctor)
    {
        _assignedDoctors.Add(doctor);
        doctor.AssignShiftDate(this);
    }

    public void RemoveDoctors() => _assignedDoctors.Clear();
}

public class ShiftScheduler
{
    private readonly List<Doctor> _doctors;
    private readonly DateTime _firstDayOfMonth;

    private List<Shift> _shifts { get; set; }
    private List<DayOfWeek>? _daysWithTwoDoctors { get; set; }

    public ShiftScheduler(List<Doctor> doctors, DateTime firstDayOfMonth, List<DayOfWeek> daysWithTwoDoctors)
    {
        _doctors = doctors;
        _firstDayOfMonth = firstDayOfMonth;
        _daysWithTwoDoctors = daysWithTwoDoctors;
        _shifts = GenerateShifts();
    }

    public List<Shift> AssignShifts()
    {
        ClearData();
        foreach (var shift in _shifts)
        {
            if (shift.RequireTwoDoctors)
            {
                FindDoctor(shift);
            }
            FindDoctor(shift);
        }

        return _shifts;
    }

    private void FindDoctor(Shift shift)
    {
        var availableDoctor = FindAvailableDoctor(shift);
        if (availableDoctor == null) return;

        shift.AssignDoctor(availableDoctor);
    }

    private List<Shift> GenerateShifts()
    {
        var lastDayOfMonth = _firstDayOfMonth.AddMonths(1).AddDays(-1);

        var shifts = new List<Shift>();
        for (var date = _firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
        {
            shifts.Add(new Shift
            {
                Date = date,
                RequireTwoDoctors = _daysWithTwoDoctors != null && _daysWithTwoDoctors.Contains(date.DayOfWeek)
            });
        }
        return shifts;
    }

    private Doctor? FindAvailableDoctor(Shift shift)
    {
        return Shuffle(_doctors)
            .FirstOrDefault(d => d.IsDoctorAvailableForShift(shift))!;

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

    private void ClearData()
    {
        _shifts.ForEach(x => x.RemoveDoctors());
        _doctors.ForEach(x => x.ResetAssignedDates());
    }
}
