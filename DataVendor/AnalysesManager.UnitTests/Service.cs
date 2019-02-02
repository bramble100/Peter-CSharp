using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace AnalysesManager.UnitTests
{
    [TestFixture]
    public class Service
    {
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
            Services.Implementations.Service.ContainsDataWithoutIsin(inputMarketData).Should().BeTrue();
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
            Services.Implementations.Service.ContainsDataWithoutIsin(inputMarketData).Should().BeFalse();
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

            Services.Implementations.Service.RemoveEntriesWithoutUptodateData(testMarketData, DateTime.Now.Date);
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