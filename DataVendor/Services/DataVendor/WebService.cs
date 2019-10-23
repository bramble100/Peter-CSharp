using Infrastructure;
using Models.Interfaces;
using NLog;
using Repositories.Interfaces;
using Services.DataVendor.Html;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Services.DataVendor
{
    public class WebService : IWebService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IEnvironmentVariableReader _environmentVariableReader;
        private readonly IHttpFacade _httpFacade;
        private readonly IMarketDataRepository _marketDataCsvFileRepository;

        public WebService(
            IEnvironmentVariableReader environmentVariableReader,
            IHttpFacade httpFacade,
            IMarketDataRepository marketDataRepository)
        {
            _environmentVariableReader = environmentVariableReader
                ?? throw new ArgumentNullException(nameof(environmentVariableReader));
            _httpFacade = httpFacade
                ?? throw new ArgumentNullException(nameof(httpFacade));
            _marketDataCsvFileRepository = marketDataRepository
                ?? throw new ArgumentNullException(nameof(marketDataRepository));
        }

        public async Task UpdateMarketData()
        {
            var latestData = await GetDownloadedDataFromWeb() ?? throw new ServiceException("No market data to add.");

            if (!latestData.Any())
            {
                _logger.Warn("No market data to add.");
                return;
            }

            _logger.Info("Updating and saving market data ...");

            var entities = latestData.ToImmutableArray();

            _marketDataCsvFileRepository.AddRange(entities);
            _logger.Info("Market data updated.");

            _marketDataCsvFileRepository.SaveChanges();
            _logger.Info("Market data saved.");
        }

        private async Task<IEnumerable<IMarketDataEntity>> GetDownloadedDataFromWeb()
        {
            _logger.Info("Downloading market data ...");

            var entities = new List<IMarketDataEntity>();

            using (var client = _httpFacade.GetHttpClient())
            {
                foreach (var link in Links)
                {
                    _logger.Info($"Downloading: {link.Key}");

                    string htmlContent = await client.GetStringAsync(link.Value);
                    if (string.IsNullOrWhiteSpace(htmlContent))
                    {
                        _logger.Warn("No html content was downloaded.");
                        continue;
                    }

                    var foundEntities = HtmlProcessor.GetMarketDataEntities(htmlContent, link.Key).ToImmutableArray();
                    if (!foundEntities.Any())
                    {
                        _logger.Warn("No market data was parsed from html content.");
                        continue;
                    }

                    _logger.Info($"{link.Key}: {foundEntities.Count()}");
                    entities.AddRange(foundEntities);
                }
            }

            _logger.Info($"{(entities.Any() ? entities.Count.ToString() : "No")} market data entity downloaded.");

            return entities;
        }

        private Dictionary<string, Uri> Links => _environmentVariableReader
            .GetEnvironmentVariable("PeterDataVendorLinks")
            .Split(';')
            .Select(item => GetUriKeyValuePair(item))
            .ToDictionary(item => item.Key, item => item.Value);

        private static KeyValuePair<string, Uri> GetUriKeyValuePair(string input)
        {
            var delimiterPosition = input.IndexOf('=');
            return new KeyValuePair<string, Uri>(
                input.Substring(0, delimiterPosition),
                new Uri(input.Substring(delimiterPosition + 1)));
        }
    }
}
