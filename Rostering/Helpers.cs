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

    public static DateTime ConvertToFirstDayOfMonth(this string month)
    {
        string[] months = ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];
        var monthNumber = Array.IndexOf(months, month.ToLower()) + 1;
        return new DateTime(DateTime.Now.Year, monthNumber, 1);
    }

    public static DateTime ConvertToDate(this int date, DateTime currentMonth)
    {
        return new DateTime(currentMonth.Year, currentMonth.Month, date);
    }
}