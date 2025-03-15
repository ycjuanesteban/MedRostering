using VYaml.Annotations;

namespace Rostering.Application.Models;

[YamlObject]
public partial class RosteringConfiguration
{
    [YamlMember("mes")]
    public string Month { get; set; } = default!;
    
    [YamlMember("medicos")]
    public IEnumerable<DoctorData> Doctors { get; set; } = default!;

    [YamlMember("dias_con_dos_medicos")]
    public List<string> DaysWithTwoDoctors { get; set; } = default!;

    [YamlMember("cantidad_guardias_a_generar")]
    public int ShiftQuantity { get; set; } = 5;
}

[YamlObject]
public partial class DoctorData
{
    [YamlMember("nombre")]
    public string Name { get; set; }

    [YamlMember("peticiones")]
    public RequestData? Requests { get; set; }
}
[YamlObject]
public partial class RequestData
{
    [YamlMember("dias_sin_guardia")]
    public List<string>? DaysOfWeekOff { get; set; }

    [YamlMember("dias_peticion")]
    public List<int>? DaysOff { get; set; }

    [YamlMember("vacaciones")]
    public Vacations? Vacations { get; set; }
}

[YamlObject]
public partial class Vacations
{
    [YamlMember("inicio")]
    public int Start { get; set; }

    [YamlMember("fin")]
    public int End { get; set; }
}