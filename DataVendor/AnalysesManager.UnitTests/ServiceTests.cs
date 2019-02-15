using AnalysesManager.Services.Implementations;
using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Builders;
using Peter.Models.Enums;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace AnalysesManager.UnitTests
{
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void GetTaz_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Service.GetTAZ(null));

            var analysis = new AnalysisBuilder()
                .SetClosingPrice(1)
                .SetFinancialAnalysis(null)
                .SetName("test")
                .SetQtyInBuyingPacket(1)
                .SetTechnicalAnalysis(null)
                .Build();
            Assert.Throws<ArgumentNullException>(() => Service.GetTAZ(analysis));
        }

        [TestCase(0, 1, 1)]
        [TestCase(-1, 1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(1, 1, 0)]
        [TestCase(1, 1, -1)]
        public void GetTaz_ThrowsArgumentException(decimal closingPrice, decimal fastSMA, decimal slowSMA)
        {
            var analysis = new AnalysisBuilder()
                .SetClosingPrice(1)
                .SetFinancialAnalysis(null)
                .SetName("test")
                .SetQtyInBuyingPacket(1)
                .SetTechnicalAnalysis(new TechnicalAnalysisBuilder()
                    .SetFastSMA(1)
                    .SetSlowSMA(1)
                    .Build())
                .Build();
            analysis.ClosingPrice = closingPrice;
            analysis.TechnicalAnalysis.FastSMA = fastSMA;
            analysis.TechnicalAnalysis.SlowSMA = slowSMA;

            Assert.Throws<ArgumentException>(() => Service.GetTAZ(analysis));
        }

        [TestCase(3.1, 2.1, 1.1, TAZ.AboveTAZ)]
        [TestCase(3.1, 1.1, 2.1, TAZ.AboveTAZ)]
        [TestCase(0.1, 2.1, 1.1, TAZ.BelowTAZ)]
        [TestCase(0.1, 1.1, 2.1, TAZ.BelowTAZ)]
        [TestCase(1.6, 2.1, 1.1, TAZ.InTAZ)]
        [TestCase(1.6, 1.1, 2.1, TAZ.InTAZ)]
        [TestCase(2.1, 2.1, 1.1, TAZ.InTAZ)]
        [TestCase(1.1, 2.1, 1.1, TAZ.InTAZ)]
        [TestCase(2.1, 1.1, 2.1, TAZ.InTAZ)]
        [TestCase(1.1, 1.1, 2.1, TAZ.InTAZ)]
        public void GetTaz_ReturnsCorrectResult(
            decimal closingPrice,
            decimal fastSMA,
            decimal slowSMA,
            TAZ expectedResult)
        {
            var analysis = new AnalysisBuilder()
                .SetClosingPrice(closingPrice)
                .SetFinancialAnalysis(null)
                .SetName("test")
                .SetQtyInBuyingPacket(1)
                .SetTechnicalAnalysis(new TechnicalAnalysisBuilder()
                    .SetFastSMA(fastSMA)
                    .SetSlowSMA(slowSMA)
                    .Build())
                .Build();

            Assert.AreEqual(expectedResult, Service.GetTAZ(analysis));
        }

        [Test]
        public void ContainsDataWithoutIsin_WithDataWithoutIsin_ReturnsTrue()
        {
            var inputMarketData = new List<IMarketDataEntity>
            {
                new MarketDataEntity
                {
                    Name="Keep",
                    Isin=string.Empty
                }
            };
            Service.ContainsDataWithoutIsin(inputMarketData).Should().BeTrue();
        }

        [Test]
        public void ContainsDataWithoutIsin_WithDataWithIsin_ReturnsFalse()
        {
            var inputMarketData = new List<IMarketDataEntity>
            {
                new MarketDataEntity
                {
                    Name="Keep",
                    Isin="Keep"
                }
            };
            Service.ContainsDataWithoutIsin(inputMarketData).Should().BeFalse();
        }

        [Test]
        public void RemoveEntriesWithoutUptodateData_WithOutDatedData_ReturnsCorrectData()
        {
            var testMarketData = new List<IMarketDataEntity>
            {
                new MarketDataEntity
                {
                    Name="Keep",
                    Isin="1",
                    DateTime = DateTime.Now.Date
                },
                new MarketDataEntity
                {
                    Name="Keep",
                    Isin="1",
                    DateTime = DateTime.Now.AddDays(-1).Date
                },
                new MarketDataEntity
                {
                    Name="Throw",
                    Isin="2",
                    DateTime = DateTime.Now.AddDays(-1).Date
                },
            };

            var expectedMarketData = new List<IMarketDataEntity>
            {
                new MarketDataEntity
                {
                    Name="Keep",
                    Isin="1",
                    DateTime = DateTime.Now.Date
                },
                new MarketDataEntity
                {
                    Name="Keep",
                    Isin="1",
                    DateTime = DateTime.Now.AddDays(-1).Date
                }
            };

            Service.RemoveEntriesWithoutUptodateData(testMarketData, DateTime.Now.Date);
            testMarketData.Should().Equal(expectedMarketData);
        }

        [Test]
        public void GetRegistryEntriesWithoutFinancialReport_WithMixedRegistry_ReturnsCorrectData()
        {
            var testRegistry = new List<RegistryEntry>
            {
                new RegistryEntry("Keep",new RegistryEntry
                {
                    FinancialReport = new FinancialReport(0.1m, 3, DateTime.Now.AddDays(1).Date)
                }),
                new RegistryEntry("Throw",new RegistryEntry
                {
                    FinancialReport = new FinancialReport()
                }),
            };

            var expectedResult = new List<RegistryEntry>
            {
                new RegistryEntry("Keep",new RegistryEntry
                {
                    FinancialReport = new FinancialReport(0.1m, 3, DateTime.Now.AddDays(1).Date)
                }),
            };

            // TODO investigate
            //Services.Implementations.Service.GetInterestingRegistryEntries(testRegistry).Should().Equal(expectedResult);
        }
    }
}