using GiornateMondiali.Core.Models.Base;

namespace GiornateMondiali.Core.Models
{
    public record FixedDay(int Day, int Month, string Name, string Description, List<string> Url) : DayBase(Name, Description, Url)
    {
        public override SpecialDay ToDay(int year) => new(new DateTime(year, Month, Day), Name, Description, Url);
    }
}
