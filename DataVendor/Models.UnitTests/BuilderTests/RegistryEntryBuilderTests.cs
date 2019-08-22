using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Builders;
using Peter.Models.Enums;
using System;

namespace Models.UnitTests.BuilderTests
{
    [TestFixture]
    public class RegistryEntryBuilderTests
    {
        [TestCase("OK1234567890", "Ok", "http://own.com", "http://stock.com", "noposition")]
        public void ShouldReturnValidObject_WhenValidInput(
            string isin,
            string name,
            string ownInvestorLink,
            string stockExchangeLink,
            string position)
        {
            var datetime = DateTime.Now.Date;
            var fundamentalAnalysis = new FundamentalAnalysisBuilder()
                .SetClosingPrice(1)
                .SetEPS(2.7m)
                .SetMonthsInReport(6)
                .Build();

            var financialReport = new FinancialReportBuilder()
                .SetEPS(3.8m)
                .SetMonthsInReport(3)
                .SetNextReportDate(datetime)
                .Build();

            var result = new RegistryEntryBuilder()
                .SetIsin(isin)
                .SetName(name)
                .SetOwnInvestorLink(ownInvestorLink)
                .SetStockExchangeLink(stockExchangeLink)
                .SetFundamentalAnalysis(fundamentalAnalysis)
                .SetFinancialReport(financialReport)
                .Build();

            result.Should().NotBeNull();
            result.Isin.Should().Be(isin);
            result.Name.Should().Be(name);
            result.OwnInvestorLink.Should().Be(ownInvestorLink);
            result.StockExchangeLink.Should().Be(stockExchangeLink);
            result.FundamentalAnalysis.Should().Be(fundamentalAnalysis);
            result.FinancialReport.Should().Be(financialReport);
        }

        [TestCase(-1, "Ok")] // negative price
        [TestCase(0, "Ok")] // zero price
        [TestCase(1, "")] // no name
        [TestCase(1, "\n")] // whitespace name
        public void ShouldReturnNull_WhenInvalidInput(decimal closingPrice, string name)
        {
            DateTime dateTime = new DateTime(2999, 12, 31);

            var result = new Peter.Models.Builders.MarketDataEntityBuilder()
                .SetClosingPrice(closingPrice)
                .SetDateTime(dateTime)
                .SetName(name)
                .Build();

            result.Should().BeNull();
        }

        [Test]
        public void ShouldReturnNull_WhenInvalidInputDate()
        {
            var result = new Peter.Models.Builders.MarketDataEntityBuilder()
                .SetClosingPrice(1.7m)
                .SetName("Ok")
                .Build();

            result.Should().BeNull();
        }
    }
}
