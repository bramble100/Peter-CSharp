using AnalysesManager.Services.Interfaces;
using NLog;
using Peter.Models.Builders;
using Peter.Models.Enums;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AnalysesManager.Services.Implementations
{
    public class Service : IService
    {
        protected readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IAnalysesRepository _financialAnalysesCsvFileRepository;
        private readonly IMarketDataRepository _marketDataRepository;
        private readonly IRegistryRepository _registryRepository;

        private readonly int _fastMovingAverage;
        private readonly int _slowMovingAverage;
        private readonly int _buyingPacketInEuro;

        public Service()
        {
            try
            {
                _financialAnalysesCsvFileRepository = new AnalysesCsvFileRepository();
                _marketDataRepository = new MarketDataCsvFileRepository();
                _registryRepository = new RegistryCsvFileRepository();

                var reader = new AppSettingsReader();

                _buyingPacketInEuro = (int)reader.GetValue("BuyingPacketInEuro", typeof(int));
                _fastMovingAverage = (int)reader.GetValue("FastMovingAverage", typeof(int));
                _slowMovingAverage = (int)reader.GetValue("SlowMovingAverage", typeof(int));

                _logger.Debug($"Buying Packet is {_buyingPacketInEuro} EUR from config file.");
                _logger.Debug($"Fast Moving Average subset size is {_fastMovingAverage} from config file.");
                _logger.Debug($"Slow Moving Average subset size is {_slowMovingAverage} from config file.");

                if (_fastMovingAverage >= _slowMovingAverage)
                {
                    throw new BusinessException("The timespan for the fast moving average must be lower than of the slow moving average.");
                }
                if (_buyingPacketInEuro <= 0 || _fastMovingAverage <= 0 || _slowMovingAverage <= 0)
                {
                    throw new BusinessException("Buying packet and moving average subset sizes must be positive numbers.");
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new BusinessException($"Error when initializing {GetType().Name}.", ex);
            }
        }

        public void GenerateAnalyses()
        {
            _logger.Info("Start generating analyses.");

            var marketData = _marketDataRepository.GetAll().ToList();
            if (ContainsDataWithoutIsin(marketData))
            {
                throw new BusinessException("There are marketdata without ISIN. No analysis generated.");
            }

            _logger.Info("Removing discontinued market data rows.");
            var latestDate = marketData.Max(d => d.DateTime).Date;
            RemoveEntriesWithoutUptodateData(marketData, latestDate);
            _logger.Info($"Having discontinued market data rows removed {marketData.Count} data entries remained.");

            var filteredRegistry = new Registry(
                _registryRepository
                .GetAll()
                .Where(RegistryItemIsInteresting));
            _logger.Info($"{filteredRegistry.Count()} registry entry found with positive EPS.");

            var groupedMarketData = from data in marketData
                                    where filteredRegistry.ContainsKey(data.Isin)
                                    group data by data.Isin into dataByIsin
                                    select dataByIsin
                                        .OrderByDescending(d => d.DateTime)
                                        .Take(_slowMovingAverage);

            var analyses = groupedMarketData.Select(GetAnalysis);
            _logger.Info($"{analyses.Count()} analyses generated.");

            _financialAnalysesCsvFileRepository.AddRange(analyses);
            _financialAnalysesCsvFileRepository.SaveChanges();
        }

        internal static bool ContainsDataWithoutIsin(List<IMarketDataEntity> marketData) =>
            marketData.Any(d => string.IsNullOrWhiteSpace(d.Isin));

        internal static void RemoveEntriesWithoutUptodateData(
            List<IMarketDataEntity> marketData,
            DateTime latestDate) =>
                marketData.RemoveAll(d => marketData.Where(d2 => string.Equals(d.Isin, d2.Isin)).Max(d3 => d3.DateTime).Date < latestDate);

        private bool RegistryItemIsInteresting(KeyValuePair<string, IRegistryEntry> keyValuePair) =>
            keyValuePair.Value?.FinancialReport?.EPS >= 0 || keyValuePair.Value?.Position != Position.NoPosition;

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

            // TODO add registry repository GetByIsin()
            var stockBaseData = _registryRepository
                .GetAll()
                .First(e => string.Equals(e.Key, isin))
                .Value;

            var analysis = new Analysis
            {
                Name = stockBaseData.Name,
                ClosingPrice = groupedMarketData.FirstOrDefault().ClosingPrice,
                QtyInBuyingPacket = (int)Math.Floor(_buyingPacketInEuro / groupedMarketData.FirstOrDefault().ClosingPrice),
                TechnicalAnalysis = new TechnicalAnalysisBuilder()
                    .SetFastSMA(groupedMarketData.Take(_fastMovingAverage).Average(d => d.ClosingPrice))
                    .SetSlowSMA(groupedMarketData.Take(_slowMovingAverage).Average(d => d.ClosingPrice))
                    .Build(),
                FinancialAnalysis = new FinancialAnalysisBuilder()
                    .SetMonthsInReport(stockBaseData.FinancialReport?.MonthsInReport)
                    .SetClosingPrice(groupedMarketData.FirstOrDefault().ClosingPrice)
                    .SetEPS(stockBaseData.FinancialReport?.EPS)
                    .Build()
            };

            analysis.TechnicalAnalysis.TAZ = GetTAZ(analysis);
            analysis.TechnicalAnalysis.Trend = GetTrend(analysis);

            return new KeyValuePair<string, IAnalysis>(isin, analysis); ;
        }

        private static TAZ GetTAZ(IAnalysis analysis)
        {
            if (analysis.ClosingPrice > Math.Max(
                analysis.TechnicalAnalysis.FastSMA,
                analysis.TechnicalAnalysis.SlowSMA))
            {
                return TAZ.AboveTAZ;
            }
            else if (analysis.ClosingPrice < Math.Min(
                analysis.TechnicalAnalysis.FastSMA,
                analysis.TechnicalAnalysis.SlowSMA))
            {
                return TAZ.BelowTAZ;
            }

            return TAZ.InTAZ;
        }

        private Trend GetTrend(IAnalysis analysis)
        {
            if (analysis.TechnicalAnalysis.FastSMA > analysis.TechnicalAnalysis.SlowSMA)
            {
                return Trend.Up;
            }
            else if (analysis.TechnicalAnalysis.FastSMA < analysis.TechnicalAnalysis.SlowSMA)
            {
                return Trend.Down;
            }

            return Trend.Undefined;
        }
    }
}
