using Peter.Models.Implementations;
using System;
using System.Collections.Generic;

namespace Peter.Models.Interfaces
{
    /// <summary>
    /// Collection of market data informations collected from the data vendor pages.
    /// </summary>
    public interface IMarketDataEntities : ICollection<IMarketDataEntity>
    {
        /// <summary>
        /// Adds the market data informations to the collection.
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<IMarketDataEntity> entities);
        /// <summary>
        /// Performs the specified action on each element of the collection.
        /// </summary>
        /// <param name="action"></param>
        void ForEach(Action<IMarketDataEntity> action);
        /// <summary>
        /// Sorts the market data informations.
        /// </summary>
        void Sort();
    }
}