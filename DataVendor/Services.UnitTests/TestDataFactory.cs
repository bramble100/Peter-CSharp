using Models.Builders;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.UnitTests
{
    /// <summary>
    /// Provides random data for testing. The data vary by each run!
    /// </summary>
    internal class TestDataFactory
    {
        static readonly Random _random = new Random();

        internal static IEnumerable<string> NewIsins(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return $"XX{Guid.NewGuid().ToString("N").Substring(0, 10)}";
            }
        }

        internal static IEnumerable<string> NewCompanyNames(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return $"TestCompany #{Guid.NewGuid().ToString("N")}";
            }
        }

        internal static IEnumerable<IMarketDataEntity> NewMarketData(IEnumerable<string> companies, int daysCounter)
        {
            var now = DateTime.Now;

            foreach (var company in companies)
            {
                for (int i = 0; i < daysCounter; i++)
                {
                    yield return new MarketDataEntityBuilder()
                        .SetClosingPrice(NewClosingPrice())
                        .SetDateTime(now.AddDays(i * -1))
                        .SetName(company)
                        .SetPreviousDayClosingPrice(NewClosingPrice())
                        .Build();
                }
            }
        }

        internal static IEnumerable<IRegistryEntry> NewRegistryEntries(IEnumerable<string> isins, IEnumerable<string> names)
        {
            var isinsArray = isins.ToArray();
            var namesArray = names.ToArray();

            for (int i = 0; i < Math.Min(isinsArray.Length, namesArray.Length); i++)
            {
                if (string.IsNullOrWhiteSpace(isinsArray[i]) || string.IsNullOrWhiteSpace(namesArray[i]))
                {
                    continue;
                }

                yield return new RegistryEntryBuilder()
                    .SetFinancialReport(NewFinancialReport())
                    .SetFundamentalAnalysis(NewFundamentalAnalysis())
                    .SetIsin(isinsArray[i])
                    .SetName(namesArray[i])
                    .Build();
            }
        }

        private static decimal NewClosingPrice() => (decimal)Math.Round(_random.NextDouble() * 1000 + 1, 2);

        private static decimal NewEPS() => (decimal)Math.Round(_random.NextDouble() * 1000 - 500, 2);

        private static IFinancialReport NewFinancialReport() =>
            new FinancialReportBuilder()
                .SetEPS(NewEPS())
                .SetMonthsInReport(3)
                .SetNextReportDate(DateTime.Now)
                .Build();

        private static IFundamentalAnalysis NewFundamentalAnalysis() =>
            new FundamentalAnalysisBuilder()
                .SetClosingPrice(NewClosingPrice())
                .SetEPS(NewEPS())
                .SetMonthsInReport(3)
                .Build();
    }
}
