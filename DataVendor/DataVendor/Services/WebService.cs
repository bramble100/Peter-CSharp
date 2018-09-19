using DataVendor.Repositories.Implementations;
using Peter.Models.Interfaces;

namespace DataVendor.Services
{
    public class WebService
    {
        private readonly MarketDataCsvFileRepository _marketDataCsvFileRepository;

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

        internal void Update(IMarketDataEntities latestData) => _marketDataCsvFileRepository.Update(latestData);
    }
}
