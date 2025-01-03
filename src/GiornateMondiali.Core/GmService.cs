using GiornateMondiali.Core.Models;

namespace GiornateMondiali.Core;

public class GmService : IGmService
{
    private const string fixedDays = "fixed.csv";
    private const string variableDays = "variables.csv";
    private const string everyXyearsDays = "everyXyears.csv";

    private List<SpecialDay>? cachedSpecialDays;

    public SpecialDayResponse GetSpecialDays(DateTime date)
    {
        if (cachedSpecialDays == default)
            cachedSpecialDays = GetSpecialDays(date.Year);

        var today = cachedSpecialDays.Where(x => x.Date.Date == date.Date).ToList();
        if (today.Count == 0) today.Add(new SpecialDay(date, "Oggi niente", "", []));
        var next = cachedSpecialDays.Where(x => x.Date.Date > date.Date).Take(5).ToList();

        return new SpecialDayResponse(today, next);
    }

    private static List<SpecialDay> GetSpecialDays(int year)
    {
        var specialDays = new List<SpecialDay>();

        var fixedDaysList = ReadFixedDays().ConvertAll(x => x.ToDay(year));
        var variableDaysList = ReadVariableDays().ConvertAll(x => x.ToDay(year));
        var everyXyearsDaysList = ReadEveryXYears().Select(x => x.ToDay(year)).Where(x => x != default).ToList();

        specialDays.AddRange(fixedDaysList);
        specialDays.AddRange(variableDaysList);
        specialDays.AddRange(everyXyearsDaysList);

        specialDays.Sort((a, b) => a.Date.CompareTo(b.Date));

        return specialDays;
    }

    private static List<FixedDay> ReadFixedDays()
    {
        List<FixedDay> days = [];
        foreach (var line in ReadFile(fixedDays))
        {
            var date = line[0].Split('/');
            days.Add(new FixedDay(
                Day: Convert.ToInt32(date[0]),
                Month: Convert.ToInt32(date[1]),
                Name: line[1],
                Description: line[2],
                Url: [.. line[3..]]
                ));
        }

        return days;
    }

    private static List<VariableDay> ReadVariableDays()
    {
        List<VariableDay> days = [];
        foreach (var line in ReadFile(variableDays))
        {
            var date = line[0].Split('/');
            var day = date[0].Split('-');

            days.Add(new VariableDay(
                WeekOfTheMonth: Convert.ToInt32(day[0]),
                DayOfTheWeek: Convert.ToInt32(day[1]),
                Month: Convert.ToInt32(date[1]),
                Name: line[1],
                Description: line[2],
                Url: [.. line[3..]]
                ));
        }

        return days;
    }

    private static List<EveryXYearDay> ReadEveryXYears()
    {
        List<EveryXYearDay> days = [];
        foreach (var line in ReadFile(everyXyearsDays))
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
                Url: [.. line[3..]]
                ));
        }

        return days;
    }

    private static IEnumerable<string[]> ReadFile(string fileName) =>
        File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName))
            .Where(line => !line.StartsWith('#'))
            .Select(a => a.Split(';'));
}
