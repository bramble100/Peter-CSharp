using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;
using NLog;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Services.DataVendor.Html;

namespace Services.DataVendor
{
    public class WebService : IWebService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMarketDataRepository _marketDataCsvFileRepository;

        public WebService(IMarketDataRepository marketDataRepository)
        {
            _marketDataCsvFileRepository = marketDataRepository;
        }

        public IEnumerable<IMarketDataEntity> GetDownloadedDataFromWeb()
        {
            _logger.Info("Downloading market data ...");

            var entities = HtmlDownloader
                .DownloadAll()
                .GetMarketDataEntities()
                .ToImmutableList();

            _logger.Info($"{(entities.Any() ? entities.Count.ToString() : "No")} market data entity downloaded.");

            return entities;
        }

        public void Update(IEnumerable<IMarketDataEntity> latestData)
        {
            _logger.Info("Updating and saving market data ...");

            var entities = latestData.ToImmutableList();

            if (entities.Any())
            {
                _marketDataCsvFileRepository.AddRange(entities);
                _logger.Info("Market data updated.");

                _marketDataCsvFileRepository.SaveChanges();
                _logger.Info("Market data saved.");
            }
            else
            {
                _logger.Info("No market data to add.");
            }
        }
    }
}
