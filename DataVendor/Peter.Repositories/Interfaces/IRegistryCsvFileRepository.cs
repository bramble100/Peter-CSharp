using System.Collections.Generic;
using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IRegistryCsvFileRepository
    {
        IRegistry Entities { get; }

        void AddRange(IEnumerable<KeyValuePair<string, IRegistryEntry>> newEntries);
        void RemoveRange(IEnumerable<string> isins);
        /// <summary>
        /// Saves the entities into CSV file.
        /// </summary>
        void SaveChanges();
    }
}