using Peter.Repositories.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;
using NLog;

namespace DataVendor.Services
{
    public class WebService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMarketDataRepository _marketDataCsvFileRepository;

        public WebService()
        {
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
        }

        internal IMarketDataEntities DownloadFromWeb()
        {
            return HtmlDownloader
                .DownloadAll()
                .GetMarketDataEntities();
        }

        internal void Update(IMarketDataEntities latestData)
        {
            _marketDataCsvFileRepository.AddRange(latestData);

            _marketDataCsvFileRepository.SaveChanges();
            _logger.Info("Market data saved.");
        }
    }
}
