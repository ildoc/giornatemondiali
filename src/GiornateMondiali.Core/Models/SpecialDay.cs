namespace GiornateMondiali.Core.Models;

public record SpecialDay(DateTime Date, string Name, string Description, List<string> Url);

public record SpecialDayResponse(List<SpecialDay> SpecialDays, List<SpecialDay> NextSpecialDays);
