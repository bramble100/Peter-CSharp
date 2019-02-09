using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;
using NLog;
using DataVendor.Services.Html;

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

        public IMarketDataEntities DownloadFromWeb()
        {
            return HtmlDownloader
                .DownloadAll()
                .GetMarketDataEntities();
        }

        public void Update(IMarketDataEntities latestData)
        {
            _marketDataCsvFileRepository.AddRange(latestData);

            _marketDataCsvFileRepository.SaveChanges();
            _logger.Info("Market data saved.");
        }
    }
}
