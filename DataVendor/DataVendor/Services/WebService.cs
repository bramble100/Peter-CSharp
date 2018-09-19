using DataVendor.Models;
using DataVendor.Repositories;
using Peter.Models.Implementations;

namespace DataVendor.Services
{
    public class WebService
    {
        private readonly MarketDataCsvFileRepository _marketDataCsvFileRepository;

        public WebService()
        {
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
        }

        internal MarketDataEntities DownloadFromWeb()
        {
            return HtmlDownloader
                .DownloadAll()
                .GetMarketDataEntities();
        }

        internal void Update(MarketDataEntities latestData) => _marketDataCsvFileRepository.Update(latestData);
    }
}
