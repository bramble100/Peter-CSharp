using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AnalysesManager.Services
{
    public class Service
    {
        private readonly IFinancialAnalysesCsvFileRepository _financialAnalysesCsvFileRepository;
        private readonly IMarketDataCsvFileRepository _marketDataCsvFileRepository;
        private readonly IRegistryCsvFileRepository _registryCsvFileRepository;

        private readonly int FastMovingAverage;
        private readonly int SlowMovingAverage;

        public Service()
        {
            _financialAnalysesCsvFileRepository = new FinancialAnalysesCsvFileRepository();
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
            _registryCsvFileRepository = new RegistryCsvFileRepository();

            var reader = new AppSettingsReader();
            FastMovingAverage = (int)reader.GetValue("FastMovingAverage", typeof(int));
            SlowMovingAverage = (int)reader.GetValue("SlowMovingAverage", typeof(int));

            if (FastMovingAverage >= SlowMovingAverage)
            {
                throw new Exception($"The timespan for the fast moving average ({FastMovingAverage}) " +
                    $"must be lower than of the slow moving average ({SlowMovingAverage}).");
            }
        }

        public void GenerateAnalyses()
        {
            var marketData = _marketDataCsvFileRepository.Load().ToList();
            if (ContainsDataWithoutIsin(marketData))
            {
                Console.WriteLine("ERROR: There are marketdata without ISIN. No analysis generated.");
                return;
            }

            var latestDate = marketData.Max(d => d.DateTime).Date;
            RemoveEntriesWithoutUptodateData(marketData, latestDate);
            var registry = GetRegistryEntriesWithoutFinancialReport(_registryCsvFileRepository.Entities);

            var groupedMarketData = from data in marketData
                                    where registry.ContainsKey(data.Isin)
                                    group data by data.Isin into dataByIsin
                                    select dataByIsin.OrderByDescending(d => d.DateTime).Take(SlowMovingAverage);

            _financialAnalysesCsvFileRepository.AddRange(groupedMarketData.Select(GetAnalysis));
            _financialAnalysesCsvFileRepository.SaveChanges();
        }

        internal static bool ContainsDataWithoutIsin(List<IMarketDataEntity> marketData) =>
            marketData.Any(d => string.IsNullOrWhiteSpace(d.Isin));

        internal static void RemoveEntriesWithoutUptodateData(
            List<IMarketDataEntity> marketData,
            DateTime latestDate) =>
                marketData.RemoveAll(d => marketData.Where(d2 => string.Equals(d.Isin, d2.Isin)).Max(d3 => d3.DateTime).Date < latestDate);

        internal static IRegistry GetRegistryEntriesWithoutFinancialReport(IRegistry registry) =>
            new Registry(registry.Where(entry => HasValidFinancialReport(entry)));

        private static bool HasValidFinancialReport(KeyValuePair<string, IRegistryEntry> entry)
        {
            return entry.Value?.FinancialReport != null &&
                entry.Value.FinancialReport.EPS != 0 &&
                !entry.Value.FinancialReport.IsOutdated;
        }

        private KeyValuePair<string, IFinancialAnalysis> GetAnalysis(IEnumerable<IMarketDataEntity> groupedMarketData)
        {
            if (groupedMarketData == null)
            {
                throw new ArgumentNullException(nameof(groupedMarketData));
            }
            var isin = groupedMarketData.First().Isin;

            var stockBaseData = _registryCsvFileRepository
                .Entities
                .First(e => string.Equals(e.Key, isin))
                .Value;

            return new KeyValuePair<string, IFinancialAnalysis>(isin, new FinancialAnalysis
            {
                ClosingPrice = groupedMarketData.FirstOrDefault().ClosingPrice,
                FastSMA = groupedMarketData.Take(FastMovingAverage).Average(d => d.ClosingPrice),
                Name = stockBaseData.Name,
                SlowSMA = groupedMarketData.Average(d => d.ClosingPrice),
                PE = groupedMarketData.FirstOrDefault().ClosingPrice / stockBaseData.FinancialReport.EPS
            });
        }
    }
}
