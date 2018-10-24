using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IIsinsRepository
    {
        /// <summary>
        /// Saves the entities into CSV file.
        /// </summary>
        /// <param name="isins"></param>
        void SaveChanges(INameToIsin isins);

        INameToIsin GetAll();
    }
}