using Peter.Models.Enums;
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
        private readonly IAnalysesCsvFileRepository _financialAnalysesCsvFileRepository;
        private readonly IMarketDataCsvFileRepository _marketDataCsvFileRepository;
        private readonly IRegistryCsvFileRepository _registryCsvFileRepository;

        private readonly int FastMovingAverage;
        private readonly int SlowMovingAverage;

        public Service()
        {
            _financialAnalysesCsvFileRepository = new AnalysesCsvFileRepository();
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
            Console.Write("Loading market data ...");
            var marketData = _marketDataCsvFileRepository.Load().ToList();
            if (ContainsDataWithoutIsin(marketData))
            {
                Console.WriteLine("ERROR: There are marketdata without ISIN. No analysis generated.");
                return;
            }

            Console.Write($" {marketData.Count} data entries loaded.\n");


            Console.Write("Removing discontinued market data rows ...");
            var latestDate = marketData.Max(d => d.DateTime).Date;
            RemoveEntriesWithoutUptodateData(marketData, latestDate);
            Console.Write($" {marketData.Count} data entries remained.\n");

            Console.Write("Loading registry ...");
            var localRegistry = GetRegistryEntriesWithFinancialReport(_registryCsvFileRepository.Entities);
            Console.Write($" {localRegistry.Count} entries loaded.\n");

            Console.WriteLine("Grouping market data.");
            var groupedMarketData = from data in marketData
                                    where localRegistry.ContainsKey(data.Isin)
                                    group data by data.Isin into dataByIsin
                                    select dataByIsin.OrderByDescending(d => d.DateTime).Take(SlowMovingAverage);

            Console.WriteLine("Saving analyses.");
            _financialAnalysesCsvFileRepository.AddRange(groupedMarketData.Select(GetAnalysis));
            _financialAnalysesCsvFileRepository.SaveChanges();
        }

        internal static bool ContainsDataWithoutIsin(List<IMarketDataEntity> marketData) =>
            marketData.Any(d => string.IsNullOrWhiteSpace(d.Isin));

        internal static void RemoveEntriesWithoutUptodateData(
            List<IMarketDataEntity> marketData,
            DateTime latestDate) =>
                marketData.RemoveAll(d => marketData.Where(d2 => string.Equals(d.Isin, d2.Isin)).Max(d3 => d3.DateTime).Date < latestDate);

        internal static IRegistry GetRegistryEntriesWithFinancialReport(IRegistry registry) =>
            new Registry(registry.Where(entry => HasValidFinancialReport(entry)));

        private static bool HasValidFinancialReport(KeyValuePair<string, IRegistryEntry> entry)
        {
            return entry.Value?.FinancialReport != null &&
                entry.Value.FinancialReport.EPS != 0 &&
                entry.Value.FinancialReport.MonthsInReport != 0;
        }

        private KeyValuePair<string, IAnalysis> GetAnalysis(IEnumerable<IMarketDataEntity> groupedMarketData)
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

            var analysis = new Analysis
            {
                Name = stockBaseData.Name,
                ClosingPrice = groupedMarketData.FirstOrDefault().ClosingPrice,
                TechnicalAnalysis = new TechnicalAnalysis
                {
                    FastSMA = groupedMarketData.Take(FastMovingAverage).Average(d => d.ClosingPrice),
                    SlowSMA = groupedMarketData.Take(SlowMovingAverage).Average(d => d.ClosingPrice)
                },
                FinancialAnalysis = new FinancialAnalysis
                {
                    PE = Math.Round(groupedMarketData.FirstOrDefault().ClosingPrice / stockBaseData.FinancialReport.EPS, 1)
                }
            };
            var tazUpperLimit = Math.Max(analysis.TechnicalAnalysis.FastSMA, analysis.TechnicalAnalysis.SlowSMA);
            var tazLowerLimit = Math.Min(analysis.TechnicalAnalysis.FastSMA, analysis.TechnicalAnalysis.SlowSMA);

            if (analysis.ClosingPrice > tazUpperLimit)
            {
                analysis.TechnicalAnalysis.TAZ = TAZ.AboveTAZ;
            }
            else if (analysis.ClosingPrice < tazLowerLimit)
            {
                analysis.TechnicalAnalysis.TAZ = TAZ.BelowTAZ;
            }
            else
            {
                analysis.TechnicalAnalysis.TAZ = TAZ.InTAZ;
            }

            return new KeyValuePair<string, IAnalysis>(isin, analysis); ;
        }
    }
}
