using FluentAssertions;
using Models.Builders;
using NUnit.Framework;
using System;

namespace Models.UnitTests.BuilderTests
{
    [TestFixture]
    public class MarketDataEntityBuilderTests
    {
        [TestCase(1.8, "OK1234567890", "Ok", 4.8, "stockExchange", 5)]
        public void ShouldReturnValidObject_WhenValidInput(
            decimal closingPrice,
            string isin,
            string name,
            decimal previousDayClosingPrice,
            string stockExchange,
            int volumen)
        {
            DateTime dateTime = DateTime.Now.Date;

            var result = new MarketDataEntityBuilder()
                .SetClosingPrice(closingPrice)
                .SetDateTime(dateTime)
                .SetIsin(isin)
                .SetName(name)
                .SetPreviousDayClosingPrice(previousDayClosingPrice)
                .SetStockExchange(stockExchange)
                .SetVolumen(volumen)
                .Build();

            result.Should().NotBeNull();
            result.ClosingPrice.Should().Be(closingPrice);
            result.DateTime.Should().Be(dateTime);
            result.Isin.Should().Be(isin);
            result.Name.Should().Be(name);
            result.PreviousDayClosingPrice.Should().Be(previousDayClosingPrice);
            result.StockExchange.Should().Be(stockExchange);
            result.Volumen.Should().Be(volumen);
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
