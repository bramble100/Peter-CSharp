namespace DataVendor.Controllers.Interfaces
{
    public interface IController
    {
        /// <summary>
        /// Adds missing ISINs to market data.
        /// </summary>
        void AddIsins();
        /// <summary>
        /// Downloads and saves latest market data.
        /// </summary>
        void WebToCsv();
    }
}