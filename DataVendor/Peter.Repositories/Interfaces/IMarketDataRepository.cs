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
        /// Collection of the market data entities.
        /// </summary>
        IEnumerable<IMarketDataEntity> Entities { get; }

        /// <summary>
        /// Adds new market data to the existing data.
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<IMarketDataEntity> entities);
        /// <summary>
        /// Gets all the stored entities.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMarketDataEntity> GetAll();
        /// <summary>
        /// Saves the content of the repository.
        /// </summary>
        void SaveChanges();
        void UpdateEntityWithIsin(IMarketDataEntity entity, string v);
    }
}