using Rostering.Application;
using Rostering.Application.Models;
using VYaml.Serialization;

var content = File.ReadAllBytes("configuration.yaml");
var rosteringData = YamlSerializer.Deserialize<RosteringConfiguration>(content);
var rosterService = new RosterService();

var rosterResult = rosterService.RosterDoctors(rosteringData);

foreach (var (generation,schedule) in rosterResult)
{
    foreach (var shift in schedule)
    {
        Console.WriteLine($"\n{shift.Date,-8:dd/MM/yy} -- {shift.Date.DayOfWeek.DayOfWeekToSpanish(),-9} " +
                          $"Doctores: {string.Join(",", shift.AssignedDoctors.Select(x => x.Name).ToArray())}");
    }
    
    var groupedDoctors = schedule
        .SelectMany(x => x.AssignedDoctors)
        .GroupBy(x => x.Name)
        .Select(x => new { Doctor = x.Key, Count = x.Count() }).ToList();
    foreach (var group in groupedDoctors)
    {
        Console.WriteLine($"\n{group.Doctor, -10} {group.Count} guardias");
    }
    
    Console.WriteLine("\n\n");
}

Console.ReadLine();