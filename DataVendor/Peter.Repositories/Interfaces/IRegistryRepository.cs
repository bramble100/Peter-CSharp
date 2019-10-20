using System.Collections.Generic;
using Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    /// <summary>
    /// Repository with registry entries.
    /// </summary>
    public interface IRegistryRepository
    {
        /// <summary>
        /// Collection of ISINs.
        /// </summary>
        IEnumerable<string> Isins { get; }

        /// <summary>
        /// Adds many registry entries.
        /// </summary>
        /// <param name="enumerable"></param>
        void AddRange(IEnumerable<IRegistryEntry> enumerable);
        /// <summary>
        /// Gets an entry by ISIN.
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
    }
}