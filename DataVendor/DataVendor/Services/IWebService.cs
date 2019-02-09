using Peter.Models.Interfaces;

namespace DataVendor.Services
{
    public interface IWebService
    {
        /// <summary>
        /// Returns all market data downloaded from datavendor.
        /// </summary>
        /// <returns></returns>
        IMarketDataEntities DownloadFromWeb();

        /// <summary>
        /// Updates underlying repository with the given data.
        /// </summary>
        /// <param name="p"></param>
        void Update(IMarketDataEntities latestData);
    }
}