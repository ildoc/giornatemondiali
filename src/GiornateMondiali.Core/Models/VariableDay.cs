using GiornateMondiali.Core.Models.Base;

namespace GiornateMondiali.Core.Models
{
    public record VariableDay(int WeekOfTheMonth, int DayOfTheWeek, int Month, string Name, string Description, List<string> Url) : DayBase(Name, Description, Url)
    {
        public override SpecialDay ToDay(int year)
        {
            var correctedDayOfTheWeek = DayOfTheWeek == 7 ? 0 : DayOfTheWeek;
            var start = new DateTime(year, Month, 1);
            while (start.DayOfWeek != (DayOfWeek)correctedDayOfTheWeek)
                start = start.AddDays(1);
            for (var i = 0; i < WeekOfTheMonth - 1; i++)
                start = start.AddDays(7);

            return new SpecialDay(start, Name, Description, Url);
        }
    }
}
