﻿using Models.Interfaces;
using System.Collections.Generic;

namespace Services.DataVendor
{
    /// <summary>
    /// Manages all market data together with ISINs.
    /// </summary>
    public interface IDataVendorService
    {
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