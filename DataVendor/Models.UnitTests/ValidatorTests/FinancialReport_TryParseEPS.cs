using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class FinancialReport_TryParseEPS
    {
        private readonly decimal _validResult = 2000.08m;

        private readonly string _valid = "2000,08";

        private readonly string _empty = string.Empty;
        private readonly string _invalidChar = "x";

        [Test]
        public void ReturnsTrue_WhenInputIsValid()
        {
            FinancialReport.TryParseEPS(_valid, out var result).Should().BeTrue();
            result.Should().Be(_validResult);
        }

        [Test]
        public void ReturnsFalse_WhenInputIsInValidEmpty() => 
            FinancialReport.TryParseEPS(_empty, out var result).Should().BeFalse();

        [Test]
        public void ReturnsFalse_WhenInputIsInValidChar() =>
            FinancialReport.TryParseEPS(_invalidChar, out var result).Should().BeFalse();
    }
}
