using System.Collections.Generic;
using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IRegistryRepository
    {
        /// <summary>
        /// Collection of ISINs.
        /// </summary>
        IEnumerable<string> Isins { get; }

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
        /// Gets one entry by ISIN.
        /// </summary>
        /// <param name="isin"></param>
        /// <returns></returns>
        IRegistryEntry GetById(string isin);

        /// <summary>
        /// Remove many registry entries.
        /// </summary>
        /// <param name="isins"></param>
        void RemoveRange(IEnumerable<string> isins);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();
        void AddRange(IEnumerable<IRegistryEntry> enumerable);
    }
}