using AnalysesManager.Services.Interfaces;
using Infrastructure;
using NLog;
using Peter.Models.Builders;
using Peter.Models.Enums;
using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalysesManager.Services.Implementations
{
    public class Service : IService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IAnalysesRepository _financialAnalysesCsvFileRepository;
        private readonly IMarketDataRepository _marketDataRepository;
        private readonly IRegistryRepository _registryRepository;
        private readonly IConfig _config;

        private readonly int _fastMovingAverage;
        private readonly int _slowMovingAverage;
        private readonly int _buyingPacketInEuro;

        public Service(
            IAnalysesRepository analysesRepository,
            IMarketDataRepository marketDataRepository,
            IRegistryRepository registryRepository,
            IConfig config)
        {
            try
            {
                _financialAnalysesCsvFileRepository = analysesRepository;
                _marketDataRepository = marketDataRepository;
                _registryRepository = registryRepository;
                _config = config;

                _buyingPacketInEuro = _config.GetValue<int>("BuyingPacketInEuro");
                _fastMovingAverage = _config.GetValue<int>("FastMovingAverage");
                _slowMovingAverage = _config.GetValue<int>("SlowMovingAverage");

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

            var groupedMarketData = from data in marketData
                                    group data by data.Isin into dataByIsin
                                    select dataByIsin
                                        .OrderByDescending(d => d.DateTime)
                                        .Take(_slowMovingAverage);

            var analyses = groupedMarketData.Select(GetAnalysis);
            _logger.Info($"{analyses.Count()} analyses generated.");

            _financialAnalysesCsvFileRepository.AddRange(analyses);
            _financialAnalysesCsvFileRepository.SaveChanges();

            _logger.Info("*** *** ***");
        }

        [Obsolete]
        private bool RegistryItemIsInteresting(KeyValuePair<string, IRegistryEntry> keyValuePair) =>
            keyValuePair.Value?.FinancialReport?.EPS >= 0 || keyValuePair.Value?.Position != Position.NoPosition;

        private bool RegistryItemIsInteresting(IRegistryEntry entry) =>
            entry.FinancialReport?.EPS >= 0 || entry.Position != Position.NoPosition;

        private KeyValuePair<string, IAnalysis> GetAnalysis(IEnumerable<IMarketDataEntity> marketData)
        {
            if (marketData == null)
                throw new ArgumentNullException(nameof(marketData));

            var isin = marketData.First()?.Isin;
            if (string.IsNullOrEmpty(isin))
                throw new ServiceException("No ISIN found for market data set.");

            var stockBaseData = _registryRepository.GetById(isin);
            if (stockBaseData is null)
                throw new ServiceException($"No registry entry found for {isin}");

            var financialAnalysis = new FinancialAnalysisBuilder()
                    .SetClosingPrice(marketData.FirstOrDefault().ClosingPrice)
                    .SetEPS(stockBaseData.FinancialReport?.EPS)
                    .SetMonthsInReport(stockBaseData.FinancialReport?.MonthsInReport)
                    .Build();
            var technicalAnalysis = new TechnicalAnalysisBuilder()
                    .SetFastSMA(marketData.Take(_fastMovingAverage).Average(d => d.ClosingPrice))
                    .SetSlowSMA(marketData.Take(_slowMovingAverage).Average(d => d.ClosingPrice))
                    .Build();
            var analysis = new AnalysisBuilder()
                .SetClosingPrice(marketData.FirstOrDefault().ClosingPrice)
                .SetName(stockBaseData.Name)
                .SetQtyInBuyingPacket((int)Math.Floor(_buyingPacketInEuro / marketData.FirstOrDefault().ClosingPrice))
                .SetFinancialAnalysis(financialAnalysis)
                .SetTechnicalAnalysis(technicalAnalysis)
                .Build();
                
            analysis.TechnicalAnalysis.TAZ = GetTAZ(analysis);
            analysis.TechnicalAnalysis.Trend = GetTrend(analysis);

            return new KeyValuePair<string, IAnalysis>(isin, analysis);
        }

        internal static bool ContainsDataWithoutIsin(List<IMarketDataEntity> marketData) =>
            marketData.Any(d => string.IsNullOrWhiteSpace(d.Isin));

        // TODO investigate
        internal static void RemoveEntriesWithoutUptodateData(
            List<IMarketDataEntity> marketData,
            DateTime latestDate) =>
                marketData.RemoveAll(d => marketData.Where(d2 => string.Equals(d.Isin, d2.Isin)).Max(d3 => d3.DateTime).Date < latestDate);

        private static bool HasValidFinancialReport(KeyValuePair<string, IRegistryEntry> entry)
        {
            return entry.Value?.FinancialReport != null &&
                entry.Value.FinancialReport.EPS != 0 &&
                entry.Value.FinancialReport.MonthsInReport != 0;
        }

        internal static TAZ GetTAZ(IAnalysis analysis)
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

        private static Trend GetTrend(IAnalysis analysis)
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
