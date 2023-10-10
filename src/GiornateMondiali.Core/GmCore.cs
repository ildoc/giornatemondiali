namespace GiornateMondiali.Core
{
    public static class GmCore
    {
        private const string fixedDays = "fixed.csv";
        private const string variableDays = "variables.csv";

        public static List<SpecialDay> GetSpecialDays(int year)
        {
            var fixedDaysList = ReadFixedDays().Select(x => x.ToDay(year)).ToList();
            var variableDaysList = ReadVariableDays().Select(x => x.ToDay(year)).ToList();

            fixedDaysList.AddRange(variableDaysList);
            fixedDaysList.Sort((a, b) => a.Date.CompareTo(b.Date));

            return fixedDaysList;
        }

        private static List<FixedDay> ReadFixedDays()
        {
            List<FixedDay> days = new();
            foreach (var line in File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(),"Data",fixedDays)).Select(a => a.Split(';')))
            {
                var date = line[0].Split('/');
                days.Add(new FixedDay(
                    Day: Convert.ToInt32(date[0]),
                    Month: Convert.ToInt32(date[1]),
                    Name: line[1],
                    Description: line[2],
                    Url: line[3..].ToList()
                    ));
            }

            return days;
        }

        private static List<VariableDay> ReadVariableDays()
        {
            List<VariableDay> days = new();
            foreach (var line in File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "Data", variableDays)).Select(a => a.Split(';')))
            {
                var date = line[0].Split('/');
                var day = date[0].Split('-');

                days.Add(new VariableDay(
                    WeekOfTheMonth: Convert.ToInt32(day[0]),
                    DayOfTheWeek: Convert.ToInt32(day[1]),
                    Month: Convert.ToInt32(date[1]),
                    Name: line[1],
                    Description: line[2],
                    Url: line[3..].ToList()
                    ));
            }

            return days;
        }

    }

    public abstract record DayBase(string Name, string Description, List<string> Url)
    {
        public abstract SpecialDay ToDay(int year);
    }

    public record SpecialDay(DateTime Date, string Name, string Description, List<string> Url);

    public record FixedDay(int Day, int Month, string Name, string Description, List<string> Url) : DayBase(Name, Description, Url)
    {
        public override SpecialDay ToDay(int year) => new(new DateTime(year, Month, Day), Name, Description, Url);
    }

    public record VariableDay(int WeekOfTheMonth, int DayOfTheWeek, int Month, string Name, string Description, List<string> Url) : DayBase(Name, Description, Url)
    {
        public override SpecialDay ToDay(int year)
        {
            var correctedDayOfTheWeek = DayOfTheWeek == 7 ? 0 : DayOfTheWeek;
            var start = new DateTime(year, Month, 1);
            while (start.DayOfWeek != (DayOfWeek)correctedDayOfTheWeek)
                start = start.AddDays(1);
            for (int i = 0; i < WeekOfTheMonth - 1; i++)
                start = start.AddDays(7);

            return new SpecialDay(start, Name, Description, Url);
        }
    }
}