using Peter.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.DataVendor
{
    public interface IWebService
    {
        /// <summary>
        /// Returns all market data downloaded from datavendor.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IMarketDataEntity>> GetDownloadedDataFromWeb();

        /// <summary>
        /// Updates underlying repository with the given data.
        /// </summary>
        /// <param name="p"></param>
        void Update(IEnumerable<IMarketDataEntity> latestData);
    }
}