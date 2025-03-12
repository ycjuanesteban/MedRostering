namespace Rostering;

public static class Helpers
{
    public static DayOfWeek SpanishDayToDayOfWeek(this string spanishDay)
    {
        var spanishDays = new Dictionary<string, string>()
        {
            {"lunes", "Monday"},
            {"martes", "Tuesday"},
            {"miercoles", "Wednesday"},
            {"jueves", "Thursday"},
            {"viernes", "Friday"},
            {"sabado", "Saturday"},
            {"domingo", "Sunday"},
        };

        return Enum.Parse<DayOfWeek>(spanishDays[spanishDay.ToLowerInvariant()]);
    }
}