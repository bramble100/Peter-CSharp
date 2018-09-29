using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalysesManager.Services
{
    public class Service
    {
        private readonly IFinancialAnalysesCsvFileRepository _financialAnalysesCsvFileRepository;
        private readonly IMarketDataCsvFileRepository _marketDataCsvFileRepository;
        private readonly IRegistryCsvFileRepository _registryCsvFileRepository;

        public Service()
        {
            _financialAnalysesCsvFileRepository = new FinancialAnalysesCsvFileRepository();
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
            _registryCsvFileRepository = new RegistryCsvFileRepository();
        }

        public void GenerateAnalyses()
        {
            var marketData = _marketDataCsvFileRepository.Load().ToList();
            if (ContainsDataWithoutIsin(marketData))
            {
                Console.WriteLine("ERROR: There are marketdata without ISIN. No analysis generated.");
                return;
            }

            var latestDate = marketData.Max(d => d.DateTime);
            RemoveEntriesWithoutUptodateData(marketData, latestDate);
            var registry = GetRegistryEntriesWithoutFinancialReport(_registryCsvFileRepository.Entities);

            var analyses = from data in marketData
                           group data by data.Isin into stock
                           select stock;








        }

        internal static bool ContainsDataWithoutIsin(List<IMarketDataEntity> marketData) =>
            marketData.Any(d => string.IsNullOrWhiteSpace(d.Isin));

        internal static void RemoveEntriesWithoutUptodateData(
            List<IMarketDataEntity> marketData,
            DateTime latestDate) =>
                marketData.RemoveAll(d => marketData.Where(d2 => string.Equals(d.Isin, d2.Isin)).Max(d3 => d3.DateTime) != latestDate);

        internal static IRegistry GetRegistryEntriesWithoutFinancialReport(IRegistry registry) =>
            new Registry(registry.Where(entry => HasValidFinancialReport(entry)));

        private static bool HasValidFinancialReport(KeyValuePair<string, IRegistryEntry> entry)
        {
            return entry.Value?.FinancialReport != null &&
                entry.Value.FinancialReport.EPS != 0 &&
                !entry.Value.FinancialReport.IsOutdated;
        }
    }
}
