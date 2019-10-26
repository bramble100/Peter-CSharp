using Models.Interfaces;
using System.Collections.Generic;

namespace Services.Analysis
{
    /// <summary>
    /// Contains business logic regarding the analyses.
    /// </summary>
    public interface IAnalysisService
    {
        /// <summary>
        /// Creates and returns analyses.
        /// </summary>
        IEnumerable<KeyValuePair<string, IAnalysis>> NewAnalyses();
        /// <summary>
        /// Saves analyses.
        /// </summary>
        void SaveAnalyses(IEnumerable<IAnalysis> items);
    }
}