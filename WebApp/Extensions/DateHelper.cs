using System.Globalization;

namespace WebApp.Extensions;

public static class DateHelper
{
    public static string MiladiToShamsi(DateTime dateTime)
    {
        var pc = new PersianCalendar();

        return $"{pc.GetYear(dateTime)}-{pc.GetMonth(dateTime)}-{pc.GetDayOfMonth(dateTime)}";
    }

    public static DateTime ShamsiToMiladi(string dateTimeString)
    {
        var datetime = DateTime.Parse(dateTimeString);

        return new DateTime(datetime.Year, datetime.Month, datetime.Day, new PersianCalendar());
    }
}