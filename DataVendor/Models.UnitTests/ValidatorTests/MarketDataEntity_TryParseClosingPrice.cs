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

        [TestCase("", "hu-HU", 1.8)]
        [TestCase("", "en-US", 1.8)]
        [TestCase("x", "hu-HU", 1.8)]
        [TestCase("x", "en-US", 1.8)]
        [TestCase("1.8", "hu-HU", 1.8)]
        public void ReturnsFalse_WhenInputIsInvalid(
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
                .BeFalse();
        }
    }
}
