using System.Collections.Generic;

namespace Repositories.Interfaces
{
    /// <summary>
    /// Repository with Name-ISIN key-value pairs.
    /// </summary>
    public interface IIsinsRepository
    {
        /// <summary>
        /// Adds a new name without ISIN.
        /// </summary>
        /// <param name="name"></param>
        void Add(string name);
        /// <summary>
        /// Adds new names without ISIN.
        /// </summary>
        /// <param name="name"></param>
        void AddRange(IEnumerable<string> names);
        /// <summary>
        /// Returns true if collection contains company name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ContainsName(string name);
        /// <summary>
        /// Gets ISIN by company name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string FindIsinByName(string name);
        /// <summary>
        /// Gets a collection of the company names.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetNames();
        /// <summary>
        /// Gets a collection of the company ISINs.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetIsins();
        /// <summary>
        /// Removes company name from collection.
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);
        /// <summary>
        /// Removes company names from collection.
        /// </summary>
        /// <param name="name"></param>
        void RemoveRange(IEnumerable<string> names);
        IEnumerable<string> FindNamesByIsin(string isin);
        /// <summary>
        /// Saves the content of the repository.
        /// </summary>
        void SaveChanges();
    }
}