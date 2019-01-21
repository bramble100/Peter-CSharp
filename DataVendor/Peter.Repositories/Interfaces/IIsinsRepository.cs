using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IIsinsRepository
    {
        /// <summary>
        /// Saves the entities into CSV file.
        /// </summary>
        /// <param name="isins"></param>
        void SaveChanges(INameToIsins isins);

        /// <summary>
        /// Gets all the Name-ISIN pairs.
        /// </summary>
        /// <returns></returns>
        INameToIsins GetAll();
    }
}