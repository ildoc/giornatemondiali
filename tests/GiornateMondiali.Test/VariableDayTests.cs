
using FluentAssertions;
using GiornateMondiali.Core;
using GiornateMondiali.Core.Models;

namespace GiornateMondiali.Test
{
    public class VariableDayTests
    {
        [Theory]
        [InlineData(2023, 3, 1, 1, 16)]
        [InlineData(2023, 2, 2, 10, 10)]
        [InlineData(2023, 1, 3, 11, 1)]
        [InlineData(2023, 3, 7, 10, 15)]
        public void Variable_ToDay_ShouldReturnCorrectly(int year, int weekOfTheMonth, int dayOfTheWeek, int month, int expectedDay)
        {
            var v = new VariableDay(weekOfTheMonth, dayOfTheWeek, month, "Test", "Test", new List<string>());

            v.ToDay(year).Date.Day.Should().Be(expectedDay);
        }
    }

    public class EveryXYearsTests
    {
        [Theory]
        [InlineData(1,1,5,2020,2025,true)]
        [InlineData(1,1,5,2020,2022,false)]
        public void EveryXYears_ToDay_ShouldReturnCorrectly(int day, int month, int cadence, int yearStart, int year,bool expected)
        {
            var v = new EveryXYearDay(day, month, cadence, yearStart, "Test", "Test", new List<string>());
            var result = v.ToDay(year) != default;
            result.Should().Be(expected);
        }
    }
}
