using System.Collections.Generic;

namespace Peter.Repositories.Interfaces
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
        string GetIsinByCompanyName(string name);
        /// <summary>
        /// Gets an immutable set of company names.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetNames();
        /// <summary>
        /// Removes company name from collection.
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);
        /// <summary>
        /// Saves the content of the repository.
        /// </summary>
        void SaveChanges();
    }
}