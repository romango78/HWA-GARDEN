using FluentAssertions;
using HWA.GARDEN.Utilities.Extensions;

namespace HWA.GARDEN.Utilities.Tests
{
    public class CalendarExtensionsTests
    {
        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(59, 2, 28)]
        [InlineData(60, 3, 1)]
        [InlineData(227, 8, 15)]
        [InlineData(365, 12, 31)]
        public void ShouldConvertDayOfYear_ToDate_ForNoLeapYear(int value, int expectedMonth, int expectedDay)
        {
            // Arrange
            const int Year = 2022;

            // Act
            var result = value.ToDateOnly(Year);

            // Asserts
            result.Should().HaveMonth(expectedMonth).And.HaveDay(expectedDay);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(59, 2, 28)]
        [InlineData(60, 3, 1)]
        [InlineData(227, 8, 15)]
        [InlineData(365, 12, 31)]
        public void ShouldConvertDayOfYear_ToDate_ForLeapYear(int value, int expectedMonth, int expectedDay)
        {
            // Arrange
            const int Year = 2024;

            // Act
            var result = value.ToDateOnly(Year);

            // Asserts
            result.Should().HaveMonth(expectedMonth).And.HaveDay(expectedDay);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 28, 59)]
        [InlineData(3, 1, 60)]
        [InlineData(8, 15, 227)]
        [InlineData(12, 31, 365)]
        public void ShouldConvertDateOnly_ToDayOfYear_ForNoLeapYear(int month, int day, int expectedValue)
        {
            // Arrange
            var value = new DateOnly(2022, month, day);

            // Act
            var result = value.ToDayOfYear();

            // Asserts
            result.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 28, 59)]
        [InlineData(3, 1, 60)]
        [InlineData(8, 15, 227)]
        [InlineData(12, 31, 365)]
        public void ShouldConvertDateOnly_ToDayOfYear_ForLeapYear(int month, int day, int expectedValue)
        {
            // Arrange
            var value = new DateOnly(2024, month, day);

            // Act
            var result = value.ToDayOfYear();

            // Asserts
            result.Should().Be(expectedValue);
        }
    }
}