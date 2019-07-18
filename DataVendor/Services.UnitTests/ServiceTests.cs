using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Builders;
using Peter.Models.Interfaces;
using Services.Analyses;
using System;
using System.Collections.Generic;

namespace AnalysesManager.UnitTests
{
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void ContainsDataWithoutIsin_WithDataWithoutIsin_ReturnsTrue()
        {
            var inputMarketData = new List<IMarketDataEntity>
            {
                new MarketDataEntityBuilder()
                    .SetName("Keep")
                    .SetIsin(string.Empty)
                    .Build()
            };
            Service.ContainsDataWithoutIsin(inputMarketData).Should().BeTrue();
        }

        [Test]
        public void ContainsDataWithoutIsin_WithDataWithIsin_ReturnsFalse()
        {
            var inputMarketData = new List<IMarketDataEntity>
            {
                new MarketDataEntityBuilder()
                    .SetName("Keep")
                    .SetIsin("Keep")
                    .Build()
            };
            Service.ContainsDataWithoutIsin(inputMarketData).Should().BeFalse();
        }

        [Test]
        public void GetRegistryEntriesWithoutFinancialReport_WithMixedRegistry_ReturnsCorrectData()
        {
            var testRegistry = new List<IRegistryEntry>
            {
                new RegistryEntryBuilder()
                    .SetName("Keep")
                    .SetFinancialReport(new FinancialReportBuilder()
                        .SetEPS(0.1m)
                        .SetMonthsInReport(3)
                        .SetNextReportDate(DateTime.Now.AddDays(1).Date)
                        .Build())
                    .Build(),
                new RegistryEntryBuilder()
                    .SetName("Throw")
                    .SetFinancialReport(new FinancialReportBuilder()
                        .SetEPS(0.1m)
                        .SetMonthsInReport(3)
                        .SetNextReportDate(DateTime.Now.AddDays(1).Date)
                        .Build())
                    .Build(),
                new RegistryEntryBuilder()
                    .SetFinancialReport(new FinancialReportBuilder().Build())
                    .Build()
            };

            var expectedResult = new List<IRegistryEntry>
            {
                new RegistryEntryBuilder()
                    .SetName("Keep")
                    .SetFinancialReport(new FinancialReportBuilder()
                        .SetEPS(0.1m)
                        .SetMonthsInReport(3)
                        .SetNextReportDate(DateTime.Now.AddDays(1).Date)
                        .Build())
                    .Build()
            };

            // TODO investigate
            //Services.Implementations.Service.GetInterestingRegistryEntries(testRegistry).Should().Equal(expectedResult);
        }
    }
}