using System.Collections.Generic;
using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IMarketDataRepository
    {
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
        /// Saves the entities into CSV file.
        /// </summary>
        void SaveChanges();
    }
}