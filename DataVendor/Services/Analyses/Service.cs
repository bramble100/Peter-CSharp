using Infrastructure;
using NLog;
using Peter.Models.Builders;
using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Services.Analyses
{
    public class Service : IService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IConfigReader _configReader;
        private readonly IAnalysesRepository _analysesCsvFileRepository;
        private readonly IMarketDataRepository _marketDataRepository;
        private readonly IRegistryRepository _registryRepository;

        private readonly IFundamentalAnalyser _fundamentalAnalyser;
        private readonly ITechnicalAnalyser _technicalAnalyser;

        private readonly int _fastMovingAverage;
        private readonly int _slowMovingAverage;
        private readonly int _buyingPacketInEuro;

        public Service(
            IAnalysesRepository analysesRepository,
            IMarketDataRepository marketDataRepository,
            IRegistryRepository registryRepository,
            IConfigReader config)
        {
            try
            {
                _analysesCsvFileRepository = analysesRepository;
                _marketDataRepository = marketDataRepository;
                _registryRepository = registryRepository;

                _fundamentalAnalyser = new FundamentalAnalyser();
                _technicalAnalyser = new TechnicalAnalyser();

                _configReader = config;

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
                _logger.Error(ex);
                throw new ServiceException($"Error when initializing {GetType().Name}.", ex);
            }
        }

        public void GenerateAnalyses()
        {
            _logger.Info("Generating analyses ...");

            var marketData = _marketDataRepository.GetAll().ToImmutableArray();
            if (ContainsDataWithoutIsin(marketData))
            {
                throw new ServiceException("There are marketdata without ISIN. No analysis generated.");
            }

            IEnumerable<IEnumerable<IMarketDataEntity>> groupedMarketData = GetGroupedMarketData(marketData);
            Dictionary<string, IAnalysis> analyses = GetAnalyses(groupedMarketData);

            if (!analyses.Any())
            {
                _logger.Warn("No analyses generated.");
                return;
            }

            _logger.Info($"{analyses.Count} analyses generated.");

            _logger.Debug("Adding analyses to repository ...");
            _analysesCsvFileRepository.AddRange(analyses);
            _logger.Debug("Analyses added.");

            _analysesCsvFileRepository.SaveChanges();

            _logger.Info("*** *** ***");
        }

        private Dictionary<string, IAnalysis> GetAnalyses(IEnumerable<IEnumerable<IMarketDataEntity>> groupedMarketData)
        {
            Dictionary<string, IAnalysis> analyses = new Dictionary<string, IAnalysis>();

            foreach (var marketDataGroup in groupedMarketData)
            {
                if (TryGetAnalysis(marketDataGroup, out KeyValuePair<string, IAnalysis> result))
                {
                    analyses[result.Key] = result.Value;
                }
            }

            return analyses;
        }

        private IEnumerable<IEnumerable<IMarketDataEntity>> GetGroupedMarketData(ImmutableArray<IMarketDataEntity> marketData)
        {
            var latestDate = marketData.Max(d => d.DateTime).Date;

            return from data in marketData
                   group data by data.Isin into dataByIsin
                   where dataByIsin.Max(d => d.DateTime).Date == latestDate
                   select dataByIsin
                       .OrderByDescending(d => d.DateTime)
                       .Take(_slowMovingAverage);
        }

        private bool TryGetAnalysis(IEnumerable<IMarketDataEntity> marketDataInput, out KeyValuePair<string, IAnalysis> result)
        {
            try
            {
                var marketData = marketDataInput?.ToImmutableArray()
                    ?? throw new ArgumentNullException(nameof(marketDataInput));

                if (!marketDataInput.Any())
                    throw new ArgumentException("Market data set cannot be empty", nameof(marketDataInput));

                var isin = marketData.First().Isin;
                if (string.IsNullOrEmpty(isin))
                    throw new ServiceException("No ISIN found in market data set.");

                var closingPrice = marketData.First().ClosingPrice;

                var stockBaseData = _registryRepository.GetById(isin)
                    ?? throw new ServiceException($"No registry entry found for {isin}");

                var fundamentalAnalysis = _fundamentalAnalyser.NewAnalysis(closingPrice, stockBaseData);

                var technicalAnalysis = _technicalAnalyser.NewAnalysis(marketData, _fastMovingAverage, _slowMovingAverage)
                    ?? throw new ServiceException($"No technical analysis can be created for {isin}");

                var analysis = new AnalysisBuilder()
                    .SetClosingPrice(closingPrice)
                    .SetName(stockBaseData.Name)
                    .SetQtyInBuyingPacket((int)Math.Floor(_buyingPacketInEuro / closingPrice))
                    .SetFundamentalAnalysis(fundamentalAnalysis)
                    .SetTechnicalAnalysis(technicalAnalysis)
                    .Build()
                    ?? throw new ServiceException($"No analysis can be created for {isin}");

                result = new KeyValuePair<string, IAnalysis>(isin, analysis);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Warn(ex.Message);
                _logger.Warn(ex, "No analysis can be created.");
                result = new KeyValuePair<string, IAnalysis>();
                return false;
            }
        }

        internal static bool ContainsDataWithoutIsin(IEnumerable<IMarketDataEntity> marketData) =>
            marketData.Any(d => string.IsNullOrWhiteSpace(d.Isin));
    }
}
