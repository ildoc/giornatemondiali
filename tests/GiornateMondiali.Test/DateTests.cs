
using FluentAssertions;
using GiornateMondiali.Core;

namespace GiornateMondiali.Test
{
    public class DateTests
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
}