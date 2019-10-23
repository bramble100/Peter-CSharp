using System.Threading.Tasks;

namespace Services.DataVendor
{
    public interface IWebService
    {
        /// <summary>
        /// Updates underlying repository with all market data downloaded from datavendor.
        /// </summary>
        /// <param name="p"></param>
        Task UpdateMarketData();
    }
}