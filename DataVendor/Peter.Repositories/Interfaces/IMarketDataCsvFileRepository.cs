using System.Collections.Generic;
using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IMarketDataCsvFileRepository
    {
        /// <summary>
        /// Loads a CSV file and returns its content.
        /// </summary>
        /// <returns></returns>
        IMarketDataEntities Load();

        /// <summary>
        /// Saves the entities into CSV file.
        /// </summary>
        /// <param name="entities"></param>
        void Save(IEnumerable<IMarketDataEntity> entities);

        /// <summary>
        /// Adds new market data to the existing data.
        /// </summary>
        /// <param name="latestData"></param>
        void Update(IMarketDataEntities latestData);
    }
}