using Newtonsoft.Json;
using Rostering;
using Rostering.Console.Models;
using Rostering.Domain;
using VYaml.Serialization;

var rosteringData = JsonConvert.DeserializeObject<RosteringConfiguration>(File.ReadAllText("configuration.json"));
// var content = File.ReadAllBytes("configuration.yaml");
// var  rosteringData = YamlSerializer.Deserialize<RosteringConfiguration>(content);

var scheduler =
    new ShiftScheduler(GetDoctors(rosteringData!), rosteringData!.Month, GetDaysWithTwoDoctors(rosteringData));

var possibleSchedules = new List<Tuple<int, List<Shift>>>();

for (var i = 0; i < 5; i++)
{
    possibleSchedules.Add(new Tuple<int, List<Shift>>(i, scheduler.AssignShifts()));
}


// Print results
foreach ((var generation, var schedule) in possibleSchedules)
{
    foreach (var shift in schedule)
    {
        Console.WriteLine($"Día: {shift.Date.ToShortDateString()} --- {shift.Date.DayOfWeek}, " +
                          $"Doctores: {string.Join(",", shift.AssignedDoctors.Select(x => x.Name).ToArray())}");
    }
    Console.WriteLine("\n\n");
}

// finalSchedule.SelectMany(x => x.AssignedDoctors)
//     .GroupBy(x => x.Name)
//     .Select(g => new { Doctor = g.Key, Count = g.Count() })
//     .ToList()
//     .ForEach(x => Console.WriteLine($"Doctor: {x.Doctor}, Guardias: {x.Count}"));

Console.ReadLine();
return;

static List<Doctor> GetDoctors(RosteringConfiguration rosteringData)
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
            },
        AssignedDays = 0
    }).ToList();
}

static List<DayOfWeek> GetDaysWithTwoDoctors(RosteringConfiguration rosteringData) =>
    rosteringData.DaysWithTwoDoctors.Select(x => x.SpanishDayToDayOfWeek()).ToList();