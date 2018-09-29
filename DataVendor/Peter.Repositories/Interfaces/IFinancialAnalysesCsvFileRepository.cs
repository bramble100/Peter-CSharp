using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Peter.Repositories.Interfaces
{
    public interface IFinancialAnalysesCsvFileRepository
    {
        /// <summary>
        /// Returns a copy of the entities.
        /// Key: ISIN. 
        /// Value: the analysis.
        /// </summary>
        Dictionary<string, IFinancialAnalysis> Entities { get; }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="analysis"></param>
        void Add(KeyValuePair<string, IFinancialAnalysis> analysis);
        /// <summary>
        /// Adds many entities.
        /// </summary>
        /// <param name="analysis"></param>
        void AddRange(IEnumerable<KeyValuePair<string, IFinancialAnalysis>> entities);
        /// <summary>
        /// Finds the analysis for the stock with the given ISIN.
        /// </summary>
        /// <param name="isin"></param>
        /// <returns></returns>
        IFinancialAnalysis Find(string isin);
        /// <summary>
        /// Removes an entity with the given ISIN.
        /// </summary>
        /// <param name="isin"></param>
        void Remove(string isin);
        /// <summary>
        /// Saves the analyses into CSV file.
        /// </summary>
        void SaveChanges();
    }
}
