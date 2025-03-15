namespace Rostering.Application;

public static class Extensions
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

    public static string DayOfWeekToSpanish(this DayOfWeek day)
    {
        var spanishDays = new Dictionary<DayOfWeek, string>()
        {
            {DayOfWeek.Monday, "Lunes"},
            {DayOfWeek.Tuesday, "Martes"},
            {DayOfWeek.Wednesday, "Miércoles"},
            {DayOfWeek.Thursday, "Jueves"},
            {DayOfWeek.Friday, "Viernes"},
            {DayOfWeek.Saturday, "Sábado"},
            {DayOfWeek.Sunday, "Domingo"}
        };

        return spanishDays[day];
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