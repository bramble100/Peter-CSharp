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

        public MarketDataEntities()
        {
        }

        public MarketDataEntities(IEnumerable<IMarketDataEntity> entities) : this()
        {
            _entities.AddRange(entities);
        }

        public void ForEach(Action<IMarketDataEntity> action) => _entities.ForEach(action);

        public void Sort() => _entities.Sort();

        public int Count => _entities.Count;

        public bool IsReadOnly => ((ICollection<IMarketDataEntity>)_entities).IsReadOnly;

        public IEnumerable<string> Isins => _entities.Where(e => !string.IsNullOrWhiteSpace(e.Isin)).Select(e => e.Isin).Distinct();

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

        public void Clear() => _entities.Clear();

        public bool Contains(IMarketDataEntity item) => _entities.Contains(item);

        public void CopyTo(IMarketDataEntity[] array, int arrayIndex) => _entities.CopyTo(array, arrayIndex);

        public IEnumerator<IMarketDataEntity> GetEnumerator() => ((IEnumerable<IMarketDataEntity>)_entities).GetEnumerator();

        public bool Remove(IMarketDataEntity item) => _entities.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<IMarketDataEntity>)_entities).GetEnumerator();
    }
}
