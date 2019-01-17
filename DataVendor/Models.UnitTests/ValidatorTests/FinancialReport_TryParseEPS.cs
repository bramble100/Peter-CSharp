using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class FinancialReport_TryParseEPS
    {
        [TestCase("2000,08", 2000.08)]
        public void ReturnsTrue_WhenInputIsValid(string input, decimal expected)
        {
            FinancialReport.TryParseEPS(input, out var result).Should().BeTrue();
            result.Should().Be(expected);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("x")]
        public void ReturnsFalse_WhenInputIsInvalid(string input) => 
            FinancialReport.TryParseEPS(input, out var result).Should().BeFalse();
    }
}
