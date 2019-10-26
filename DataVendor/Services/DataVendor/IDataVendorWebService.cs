using System.Threading.Tasks;

namespace Services.DataVendor
{
    /// <summary>
    /// Manages all market data that comes from the web, without ISINs.
    /// </summary>
    public interface IDataVendorWebService
    {
        /// <summary>
        /// Updates underlying repository with all market data downloaded from datavendor.
        /// </summary>
        /// <param name="p"></param>
        Task UpdateMarketData();
    }
}