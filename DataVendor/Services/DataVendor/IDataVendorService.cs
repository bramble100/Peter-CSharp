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
        /// Returns true if all company names have a corresponding ISIN and the relation is unambiguous.
        /// </summary>
        bool HaveAllMarketDataNamesIsins { get; }
        /// <summary>
        /// Finds the market data items that have data with the latest day and returns their ISIN.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> FindIsinsWithLatestMarketData();
        /// <summary>
        /// Finds the market data entries by ISIN.
        /// </summary>
        /// <param name="isin"></param>
        /// <returns></returns>
        IEnumerable<IMarketDataEntity> FindMarketDataByIsin(string isin);
    }
}