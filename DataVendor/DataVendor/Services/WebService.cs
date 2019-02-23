using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;
using NLog;
using DataVendor.Services.Html;
using System.Collections.Generic;

namespace DataVendor.Services
{
    public class WebService : IWebService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMarketDataRepository _marketDataCsvFileRepository;

        public WebService(IMarketDataRepository marketDataRepository)
        {
            _marketDataCsvFileRepository = marketDataRepository;
        }

        public IEnumerable<IMarketDataEntity> DownloadFromWeb()
        {
            return HtmlDownloader
                .DownloadAll()
                .GetMarketDataEntities();
        }

        public void Update(IEnumerable<IMarketDataEntity> latestData)
        {
            _marketDataCsvFileRepository.AddRange(latestData);

            _marketDataCsvFileRepository.SaveChanges();
            _logger.Info("Market data saved.");
        }
    }
}
