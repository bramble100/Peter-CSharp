using Models.Interfaces;
using System.Collections.Generic;

namespace Services.DataVendor
{
    /// <summary>
    /// Manages all market data together with ISINs.
    /// </summary>
    public interface IDataVendorService
    {
        /// <summary>
        /// Gets the market data entries grouped by ISIN.
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<string, IEnumerable<IMarketDataEntity>>> GetMarketDataGroupedByIsin();
        IEnumerable<string> FindIsinsWithLatestMarketData();
        /// <summary>
        /// Finds the market data entries by ISIN.
        /// </summary>
        /// <param name="isin"></param>
        /// <returns></returns>
        IEnumerable<IMarketDataEntity> FindMarketDataByIsin(string isin);
    }
}