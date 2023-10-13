namespace GiornateMondiali.Core.Models.Base
{
    public abstract record DayBase(string Name, string Description, List<string> Url)
    {
        public abstract SpecialDay ToDay(int year);
    }
}
