using Peter.Models.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Peter.Models.Implementations
{
    public class MarketDataEntities : IMarketDataEntities
    {
        private readonly List<IMarketDataEntity> _entities = new List<IMarketDataEntity>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public MarketDataEntities()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entities"></param>
        public MarketDataEntities(IEnumerable<IMarketDataEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void ForEach(Action<IMarketDataEntity> action) => _entities.ForEach(action);

        public void Sort() => _entities.Sort();

        /// <summary>
        /// Gets the number of market data informations contained in the collection.
        /// </summary>
        public int Count => _entities.Count;

        /// <summary>
        /// Gets a value indicating whether the ICollection is read-only.
        /// </summary>
        public bool IsReadOnly => ((ICollection<IMarketDataEntity>)_entities).IsReadOnly;

        /// <summary>
        /// Adds a market data information to the collection.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(IMarketDataEntity entity)
        {
            var actualOnThatDay = _entities
                .FirstOrDefault(e =>
                    string.Equals(e.Name, entity.Name) 
                    && Equals(e.DateTime.Date, entity.DateTime.Date));

            var infoIsAddable = actualOnThatDay is null;
            bool infoIsUpdateable = infoIsAddable ? false : entity.DateTime > actualOnThatDay.DateTime;

            if (infoIsUpdateable)
            {
                Remove(actualOnThatDay);
            }
            if(infoIsAddable || infoIsUpdateable)
            {
                _entities.Add(entity);
            }
        }

        public void AddRange(IEnumerable<IMarketDataEntity> entities)
        {
            entities.ToList().ForEach(Add);
            Console.WriteLine($"Number of market data records added: {entities.Count()}");
        }

        /// <summary>
        /// Removes all information from the collection.
        /// </summary>
        public void Clear() => _entities.Clear();

        public bool Contains(IMarketDataEntity item) => _entities.Contains(item);

        public void CopyTo(IMarketDataEntity[] array, int arrayIndex) => _entities.CopyTo(array, arrayIndex);

        public IEnumerator<IMarketDataEntity> GetEnumerator() => ((IEnumerable<IMarketDataEntity>)_entities).GetEnumerator();

        public bool Remove(IMarketDataEntity item) => _entities.Remove(item);

        internal DateTime MaxDateTime()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<IMarketDataEntity>)_entities).GetEnumerator();
    }
}
