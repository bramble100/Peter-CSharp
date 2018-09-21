using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IRegistryCsvFileRepository
    {
        /// <summary>
        /// Loads a CSV file and returns its content.
        /// </summary>
        /// <returns></returns>
        IRegistry Load();

        /// <summary>
        /// Saves the entities into CSV file.
        /// </summary>
        /// <param name="entities"></param>
        void Save(IRegistry entities);
    }
}