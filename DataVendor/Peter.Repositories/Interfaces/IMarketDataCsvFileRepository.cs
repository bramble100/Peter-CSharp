using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IMarketDataCsvFileRepository
    {
        /// <summary>
        /// Saves the entities into CSV file.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Adds new market data to the existing data.
        /// </summary>
        /// <param name="latestData"></param>
        void Update(IMarketDataEntities latestData);

        /// <summary>
        /// Gets all the stored entities.
        /// </summary>
        /// <returns></returns>
        IMarketDataEntities GetAll();
    }
}