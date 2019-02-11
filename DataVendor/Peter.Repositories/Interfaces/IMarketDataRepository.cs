using Peter.Models.Interfaces;
using System.Collections.Generic;

namespace Peter.Repositories.Interfaces
{
    /// <summary>
    /// Repository with market data.
    /// </summary>
    public interface IMarketDataRepository
    {
        /// <summary>
        /// Collection of the ISINs in the repository.
        /// </summary>
        IEnumerable<string> Isins { get; }

        /// <summary>
        /// Adds new market data to the existing data.
        /// </summary>
        /// <param name="latestData"></param>
        void AddRange(IMarketDataEntities latestData);
        /// <summary>
        /// Gets all the stored entities.
        /// </summary>
        /// <returns></returns>
        IMarketDataEntities GetAll();
        /// <summary>
        /// Saves the content of the repository.
        /// </summary>
        void SaveChanges();
    }
}