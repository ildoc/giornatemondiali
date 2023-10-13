using GiornateMondiali.Core.Models;

namespace GiornateMondiali.Core
{
    public static class GmCore
    {
        private const string fixedDays = "fixed.csv";
        private const string variableDays = "variables.csv";
        private const string everyXyearsDays = "everyXyears.csv";

        public static List<SpecialDay> GetSpecialDays(int year)
        {
            var specialDays = new List<SpecialDay>();

            var fixedDaysList = ReadFixedDays().Select(x => x.ToDay(year)).ToList();
            var variableDaysList = ReadVariableDays().Select(x => x.ToDay(year)).ToList();
            var everyXyearsDaysList = ReadEveryXYears().Select(x => x.ToDay(year)).Where(x => x != default).ToList();

            specialDays.AddRange(fixedDaysList);
            specialDays.AddRange(variableDaysList);
            specialDays.AddRange(everyXyearsDaysList);

            specialDays.Sort((a, b) => a.Date.CompareTo(b.Date));

            return specialDays;
        }

        private static IEnumerable<FixedDay> ReadFixedDays()
        {
            List<FixedDay> days = new();
            foreach (var line in File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "Data", fixedDays)).Select(a => a.Split(';')))
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

        private static IEnumerable<VariableDay> ReadVariableDays()
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

        private static IEnumerable<EveryXYearDay> ReadEveryXYears()
        {
            List<EveryXYearDay> days = new();
            foreach (var line in File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "Data", everyXyearsDays)).Select(a => a.Split(';')))
            {
                var period = line[0].Split('|');
                var date = period[0].Split('/');

                days.Add(new EveryXYearDay(
                    Day: Convert.ToInt32(date[0]),
                    Month: Convert.ToInt32(date[1]),
                    Cadence: Convert.ToInt32(period[1]),
                    YearStart: Convert.ToInt32(period[2]),
                    Name: line[1],
                    Description: line[2],
                    Url: line[3..].ToList()
                    ));
            }

            return days;
        }
    }
}
