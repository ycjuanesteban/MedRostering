using VYaml.Annotations;

namespace Rostering.Console.Models;

[YamlObject]
public partial class RosteringConfiguration
{
    public string Month { get; set; } = default!;
    public IEnumerable<DoctorData> Doctors { get; set; } = default!;
    public List<string> DaysWithTwoDoctors { get; set; } = default!;
}

[YamlObject]
public partial class DoctorData
{
    public string Name { get; set; }
    public RequestData? Requests { get; set; }
}
[YamlObject]
public partial class RequestData
{
    public List<string>? DaysOfWeekOff { get; set; }
    public List<int>? DaysOff { get; set; }
    public Vacations? Vacations { get; set; }
}

[YamlObject]
public partial class Vacations
{
    public int Start { get; set; }
    public int End { get; set; }
}