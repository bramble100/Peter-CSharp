using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IIsinsCsvFileRepository
    {
        /// <summary>
        /// Loads a CSV file and returns its content.
        /// </summary>
        /// <returns></returns>
        INameToIsin Load();

        /// <summary>
        /// Saves the entities into CSV file.
        /// </summary>
        /// <param name="isins"></param>
        void Save(INameToIsin isins);
    }
}