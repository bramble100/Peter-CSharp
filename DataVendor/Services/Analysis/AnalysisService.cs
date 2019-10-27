using Infrastructure.Config;
using Models.Builders;
using Models.Interfaces;
using NLog;
using Repositories.Interfaces;
using Services.DataVendor;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Services.Analysis
{
    /// <summary>
    /// Contains business logic regarding the analyses.
    /// </summary>
    public class AnalysisService : IAnalysisService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IConfigReader _configReader;
        private readonly IAnalysesRepository _analysesRepository;
        private readonly IMarketDataRepository _marketDataRepository;
        private readonly IRegistryRepository _registryRepository;
        private readonly IFundamentalAnalyser _fundamentalAnalyser;
        private readonly ITechnicalAnalyser _technicalAnalyser;
        private readonly IDataVendorService _datavendorService;

        private readonly int _fastMovingAverage;
        private readonly int _slowMovingAverage;
        private readonly int _buyingPacketInEuro;

        public AnalysisService(
            IFundamentalAnalyser fundamentalAnalyser,
            ITechnicalAnalyser technicalAnalyser,
            IDataVendorService datavendorService,
            IAnalysesRepository analysesRepository,
            IMarketDataRepository marketDataRepository,
            IRegistryRepository registryRepository,
            IConfigReader configReader)
        {
            try
            {
                _fundamentalAnalyser = fundamentalAnalyser;
                _technicalAnalyser = technicalAnalyser;

                _datavendorService = datavendorService;

                _analysesRepository = analysesRepository;
                _marketDataRepository = marketDataRepository;
                _registryRepository = registryRepository;

                _configReader = configReader;

                _buyingPacketInEuro = _configReader.Settings.BuyingPacketInEuro;
                _fastMovingAverage = _configReader.Settings.FastMovingAverage;
                _slowMovingAverage = _configReader.Settings.SlowMovingAverage;

                _logger.Debug($"Buying Packet is {_buyingPacketInEuro} EUR from config file.");
                _logger.Debug($"Fast Moving Average subset size is {_fastMovingAverage} from config file.");
                _logger.Debug($"Slow Moving Average subset size is {_slowMovingAverage} from config file.");

                if (_fastMovingAverage >= _slowMovingAverage)
                {
                    throw new ServiceException("The timespan for the fast moving average must be lower than of the slow moving average.");
                }
                if (_buyingPacketInEuro <= 0 || _fastMovingAverage <= 0 || _slowMovingAverage <= 0)
                {
                    throw new ServiceException("Buying packet and moving average subset sizes must be positive numbers.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ServiceException($"Error when initializing {GetType().Name}.", ex);
            }
        }

        /// <summary>
        /// Creates and returns analyses.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IAnalysis>> NewAnalyses()
        {
            _logger.Info("Generating analyses ...");

            var isins = _datavendorService.FindIsinsWithLatestMarketData().ToArray();

            foreach (var isin in isins)
            {
                if (TryNewAnalysis(isin, out var analysis))
                {
                    yield return new KeyValuePair<string, IAnalysis>(isin, analysis);
                }
            }

            _logger.Info("Analyses generated.");
        }

        /// <summary>
        /// Saves analyses.
        /// </summary>
        public void SaveAnalyses(IEnumerable<KeyValuePair<string, IAnalysis>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            _analysesRepository.AddRange(items);
        }

        /// <summary>
        /// Returns true if analysis can be made, and returns the analysis itself.
        /// </summary>
        /// <param name="isin"></param>
        /// <param name="analysis"></param>
        /// <returns></returns>
        internal bool TryNewAnalysis(string isin, out IAnalysis analysis)
        {
            analysis = null;

            if (string.IsNullOrWhiteSpace(isin))
            {
                return false;
            }

            try
            {

                var marketData = _datavendorService.FindMarketDataByIsin(isin)?.ToImmutableArray()
                    ?? throw new ServiceException($"No market data found for {isin}");

                if (!marketData.Any())
                {
                    throw new ServiceException($"No market data found for {isin}");
                }

                var stockBaseData = _registryRepository.FindByIsin(isin)
                    ?? throw new ServiceException($"No registry entry found for {isin}");

                analysis = NewAnalysis(marketData, stockBaseData);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Warn(ex.Message);
                _logger.Warn(ex, "No analysis can be created.");
                return false;
            }
        }

        /// <summary>
        /// Provides one analysis based on a market data set and a registry entry.
        /// </summary>
        /// <param name="marketData"></param>
        /// <param name="registryEntry"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        internal IAnalysis NewAnalysis(IEnumerable<IMarketDataEntity> marketData, IRegistryEntry registryEntry)
        {
            if (marketData is null)
            {
                throw new ArgumentNullException(nameof(marketData));
            }

            if (registryEntry is null)
            {
                throw new ArgumentNullException(nameof(registryEntry));
            }

            if (!marketData.Any())
            {
                throw new ArgumentException(nameof(marketData));
            }

            decimal closingPrice;

            try
            {
                closingPrice = marketData
                    .Single(d => d.DateTime == marketData.Max(d2 => d2.DateTime))
                    .ClosingPrice;
            }
            catch (InvalidOperationException)
            {
                throw new ServiceException($"No analysis can be created for {registryEntry.Isin}");
            }

            var fundamentalAnalysis = _fundamentalAnalyser.NewAnalysis(closingPrice, registryEntry);
            var technicalAnalysis = _technicalAnalyser.NewAnalysis(marketData, _fastMovingAverage, _slowMovingAverage);

            return new AnalysisBuilder()
                .SetClosingPrice(closingPrice)
                .SetName(registryEntry.Name)
                .SetQtyInBuyingPacket((int)Math.Floor(_buyingPacketInEuro / closingPrice))
                .SetFundamentalAnalysis(fundamentalAnalysis)
                .SetTechnicalAnalysis(technicalAnalysis)
                .Build()
                ?? throw new ServiceException($"No analysis can be created for {registryEntry.Isin}");
        }
    }
}
