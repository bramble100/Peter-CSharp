using NUnit.Framework;
using Peter.Models.Builders;
using Peter.Models.Enums;
using Peter.Models.Interfaces;
using Services.Analyses;
using System;
using System.Linq;

namespace AnalysesManager.UnitTests.Analyses
{
    [TestFixture]
    public class Service_NewAnalysis
    {
        private static readonly Random _random = new Random();

        [Test]
        public void WithValidDataSeries_ShouldReturnCorrectResult()
        {
            var isin = "Isin1";
            var name = "Company";
            var marketData = new IMarketDataEntity[]
            {
                new MarketDataEntityBuilder()
                    .SetClosingPrice(3)
                    .SetDateTime(DateTime.Now.AddDays(-4))
                    .SetIsin(isin)
                    .SetName(name)
                    .Build(),
                new MarketDataEntityBuilder()
                    .SetClosingPrice(2)
                    .SetDateTime(DateTime.Now.AddDays(-3))
                    .SetIsin(isin)
                    .SetName(name)
                    .Build(),
                new MarketDataEntityBuilder()
                    .SetClosingPrice(3)
                    .SetDateTime(DateTime.Now.AddDays(-2))
                    .SetIsin(isin)
                    .SetName(name)
                    .Build(),
                new MarketDataEntityBuilder()
                    .SetClosingPrice(7)
                    .SetDateTime(DateTime.Now.AddDays(-1))
                    .SetIsin(isin)
                    .SetName(name)
                    .Build(),
                new MarketDataEntityBuilder()
                    .SetClosingPrice(5)
                    .SetDateTime(DateTime.Now)
                    .SetIsin(isin)
                    .SetName(name)
                    .Build()
            }
            .OrderBy(item => _random.Next())
            .ToArray();

            var expectedResult = new TechnicalAnalysisBuilder()
                .SetFastSMA(5)
                .SetSlowSMA(4)
                .SetTAZ(TAZ.InTAZ)
                .SetTrend(Trend.Up)
                .Build();

            var result = new TechnicalAnalyser().NewAnalysis(marketData, 3, 5);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
