using GiornateMondiali.Core.Models.Base;

namespace GiornateMondiali.Core.Models;

public record EveryXYearDay(int Day, int Month, int Cadence, int YearStart, string Name, string Description, List<string> Url) : DayBase(Name, Description, Url)
{
    public override SpecialDay ToDay(int year)
    {
        var i = YearStart;
        while (i < year)
            i += Cadence;
        if (i != year)
            return default;
        return new SpecialDay(new DateTime(year, Month, Day), Name, Description, Url);
    }
}
