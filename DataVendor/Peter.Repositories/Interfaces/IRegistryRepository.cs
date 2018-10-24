using System.Collections.Generic;
using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IRegistryRepository
    {
        /// <summary>
        /// Add many registry entries.
        /// </summary>
        /// <param name="newEntries"></param>
        void AddRange(IEnumerable<KeyValuePair<string, IRegistryEntry>> newEntries);

        /// <summary>
        /// Gets all the entities.
        /// </summary>
        /// <returns></returns>
        IRegistry GetAll();

        /// <summary>
        /// Collection of ISINs.
        /// </summary>
        IEnumerable<string> Isins { get; }

        /// <summary>
        /// Remove many registry entries.
        /// </summary>
        /// <param name="isins"></param>
        void RemoveRange(IEnumerable<string> isins);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();
    }
}