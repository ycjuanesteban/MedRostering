namespace Rostering.Models;

public class RosteringConfiguration
{
    public string Month { get; set; } = default!;
    public IEnumerable<DoctorData> Doctors { get; set; } = default!;
    public List<string> DaysWithTwoDoctors { get; set; } = default!;
}

public class DoctorData
{
    public string Name { get; set; }
    public RequestData? Requests { get; set; }
}

public class RequestData
{
    public List<string>? DaysOfWeekOff { get; set; }
    public List<int>? DaysOff { get; set; }
    public Vacations? Vacations { get; set; }
}

public class Vacations
{
    public int Start { get; set; }
    public int End { get; set; }
}