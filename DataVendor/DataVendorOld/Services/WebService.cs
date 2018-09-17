using DataVendor.Models;
using DataVendor.Repositories;

namespace DataVendor.Services
{
    public class WebService
    {
        private readonly MarketDataCsvFileRepository _csvFileRepository;

        public WebService()
        {
            _csvFileRepository = new MarketDataCsvFileRepository();
        }

        internal MarketDataEntities DownloadFromWeb()
        {
            return HtmlDownloader
                .DownloadAll()
                .GetMarketDataEntities();
        }

        internal void Update(MarketDataEntities latestData) => _csvFileRepository.Update(latestData);
    }
}
