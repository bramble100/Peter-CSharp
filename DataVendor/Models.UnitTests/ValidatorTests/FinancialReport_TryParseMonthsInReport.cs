using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class FinancialReport_TryParseMonthsInReport
    {
        [TestCase("3", 3)]
        [TestCase("6", 6)]
        [TestCase("9", 9)]
        [TestCase("12", 12)]
        public void ReturnsTrue_WhenInputIsValid(string input, int expected)
        {
            FinancialReport.TryParseMonthsInReport(input, out var result).Should().BeTrue();
            result.Should().Be(expected);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("x")]
        [TestCase("1")]
        [TestCase("4,5")]
        [TestCase("4.5")]
        public void ReturnsFalse_WhenInputIsInvalid(string input) => 
            FinancialReport.TryParseMonthsInReport(input, out var _).Should().BeFalse();
    }
}
