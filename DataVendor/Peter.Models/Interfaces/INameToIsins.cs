using System.Collections.Generic;

namespace Peter.Models.Interfaces
{
    /// <summary>
    /// Collection to contain all the ISINs by the company name. Key: Company name; Value: ISIN
    /// </summary>
    public interface INameToIsins : IEnumerable<INameToIsin>
    {
        /// <summary>
        /// Indexer to return the ISIN by the company name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string this[string name] { get; }

        int Count { get; }

        void Add(INameToIsin keyValuePair);
        void Add(string[] input);
        bool ContainsKey(string name);
        void Remove(INameToIsin dn);
    }
}