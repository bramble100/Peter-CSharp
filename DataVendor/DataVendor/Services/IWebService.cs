using Peter.Models.Interfaces;

namespace DataVendor.Services
{
    public interface IWebService
    {
        /// <summary>
        /// Updates underlying repository with the given data.
        /// </summary>
        /// <param name="p"></param>
        void Update(IMarketDataEntities latestData);

        /// <summary>
        /// Returns all market data that was downloaded from datavendor's pages.
        /// </summary>
        /// <returns></returns>
        IMarketDataEntities DownloadFromWeb();
    }
}