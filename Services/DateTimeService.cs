using CDR_Worship.Services.Interfaces;

namespace CDR_Worship.Services
{
   public class DateTimeService : IDateTimeService
{
    public string FormatCommentDate(DateTime dateTime)
{
    var today = DateTime.Today;
    var yesterday = today.AddDays(-1);

    if (dateTime.Date == today)
        return "Hoy, " + dateTime.ToString("h:mm tt"); // Formato 12 horas con AM/PM
    else if (dateTime.Date == yesterday)
        return "Ayer, " + dateTime.ToString("h:mm tt");
    else
        return dateTime.ToString("dddd, MMM d 'at' h:mm tt"); // Formato completo
}
}
}