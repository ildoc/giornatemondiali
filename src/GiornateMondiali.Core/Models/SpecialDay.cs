namespace GiornateMondiali.Core.Models
{
    public record SpecialDay(DateTime Date, string Name, string Description, List<string> Url);

    public record SpecialDayResponse(SpecialDay SpecialDay, List<SpecialDay> NextSpecialDays);
}
