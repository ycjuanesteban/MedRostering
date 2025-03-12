using System.Globalization;
using Newtonsoft.Json;
using Rostering;
using Rostering.Models;

var rosteringData = JsonConvert.DeserializeObject<RosteringConfiguration>(File.ReadAllText("configuration.json"));
        
var scheduler = new ShiftScheduler(GetDoctors(rosteringData!), rosteringData!.Month, GetDaysWithTwoDoctors(rosteringData));
var finalSchedule = scheduler.AssignShifts();

// Print results
foreach (var shift in finalSchedule)
{
    Console.WriteLine($"Date: {shift.Date.ToShortDateString()} --- {shift.Date.DayOfWeek}, " +
                      $"Doctors: {string.Join(",", shift.AssignedDoctors.Select(x => x.Name).ToArray())}");
}

finalSchedule.SelectMany(x => x.AssignedDoctors)
    .GroupBy(x => x.Name)
    .Select(g => new { Doctor = g.Key, Count = g.Count() })
    .ToList()
    .ForEach(x => Console.WriteLine($"Doctor: {x.Doctor}, Guardias: {x.Count}"));

Console.ReadLine();
return;

static List<Doctor> GetDoctors(RosteringConfiguration rosteringData)
{
    return rosteringData!.Doctors.Select(d => new Doctor
    {
        Name = d.Name,
        Requests = d.Requests == null ? null : new Request()
        {
            DaysOff = d.Requests.DaysOff?.Select(df => DateTime.ParseExact(df, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList(),
            DaysOfWeekOff = d.Requests.DaysOfWeekOff?.Select(x => x.SpanishDayToDayOfWeek()).ToList(),
        },
        AssignedDays = 0
    }).ToList();
}
    
static List<DayOfWeek> GetDaysWithTwoDoctors(RosteringConfiguration rosteringData) =>  rosteringData.DaysWithTwoDoctors.Select(x => x.SpanishDayToDayOfWeek()).ToList();