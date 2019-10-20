using FluentAssertions;
using Models.Builders;
using Models.Enums;
using NUnit.Framework;
using System;

namespace Models.UnitTests.BuilderTests
{
    [TestFixture]
    public class TechnicalAnalysisBuilderTests
    {
        [TestCase(2.3, 6.5, TAZ.BelowTAZ, Trend.Down)]
        public void ShouldReturnValidObject_WhenValidInput(decimal fastSMA, decimal slowSMA, TAZ taz, Trend trend)
        {
            var result = new TechnicalAnalysisBuilder()
                .SetFastSMA(fastSMA)
                .SetSlowSMA(slowSMA)
                .SetTAZ(taz.ToString())
                .SetTrend(trend.ToString())
                .Build();

            result.Should().NotBeNull();
            result.FastSMA.Should().Be(fastSMA);
            result.SlowSMA.Should().Be(slowSMA);
            result.TAZ.Should().Be(taz);
            result.Trend.Should().Be(trend);
        }

        [TestCase(-1, "Ok")] // negative price
        [TestCase(0, "Ok")] // zero price
        [TestCase(1, "")] // no name
        [TestCase(1, "\n")] // whitespace name
        public void ShouldReturnNull_WhenInvalidInput(decimal closingPrice, string name)
        {
            DateTime dateTime = new DateTime(2999, 12, 31);

            var result = new MarketDataEntityBuilder()
                .SetClosingPrice(closingPrice)
                .SetDateTime(dateTime)
                .SetName(name)
                .Build();

            result.Should().BeNull();
        }

        [Test]
        public void ShouldReturnNull_WhenInvalidInputDate()
        {
            var result = new MarketDataEntityBuilder()
                .SetClosingPrice(1.7m)
                .SetName("Ok")
                .Build();

            result.Should().BeNull();
        }
    }
}
