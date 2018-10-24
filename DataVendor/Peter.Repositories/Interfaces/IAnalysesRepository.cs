using Peter.Models.Interfaces;
using System.Collections.Generic;

namespace Peter.Repositories.Interfaces
{
    public interface IAnalysesRepository
    {
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="analysis"></param>
        void Add(KeyValuePair<string, IAnalysis> analysis);

        /// <summary>
        /// Adds many entities.
        /// </summary>
        /// <param name="analysis"></param>
        void AddRange(IEnumerable<KeyValuePair<string, IAnalysis>> entities);

        /// <summary>
        /// Returns a copy of the entities.
        /// Key: ISIN. 
        /// Value: the analysis.
        /// </summary>
        Dictionary<string, IAnalysis> GetAll();

        /// <summary>
        /// Finds the analysis for the stock with the given ISIN.
        /// </summary>
        /// <param name="isin"></param>
        /// <returns></returns>
        IFinancialAnalysis Find(string isin);

        /// <summary>
        /// Saves the analyses into CSV file.
        /// </summary>
        void SaveChanges();
    }
}
