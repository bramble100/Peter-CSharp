using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;
using System.Globalization;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class MarketDataEntity_TryParseClosingPrice
    {
        [TestCase("1,8", "hu-HU", 1.8)]
        [TestCase("1.8", "en-US", 1.8)]
        public void ReturnsTrue_WhenInputIsValid(
            string input,
            string cultureInfoString,
            decimal expected)
        {
            MarketDataEntity
                .TryParseClosingPrice(
                    input,
                    new CultureInfo(cultureInfoString),
                    out var result)
                .Should()
                .BeTrue();
            result.Should().Be(expected);
        }

        [TestCase(null, null)]
        [TestCase(null, "hu-HU")]
        [TestCase("", "hu-HU")]
        [TestCase("", "en-US")]
        [TestCase("x", "hu-HU")]
        [TestCase("x", "en-US")]
        [TestCase("1.8", "hu-HU")]
        [TestCase("0", "hu-HU")]
        [TestCase("-4", "hu-HU")]
        public void ReturnsFalse_WhenInputIsInvalid(
            string input,
            string cultureInfoString)
        {
            var cultureInfo = string.IsNullOrEmpty(cultureInfoString) 
                ? null 
                : new CultureInfo(cultureInfoString);
            MarketDataEntity
                .TryParseClosingPrice(input, cultureInfo, out var _)
                .Should()
                .BeFalse();
        }
    }
}
