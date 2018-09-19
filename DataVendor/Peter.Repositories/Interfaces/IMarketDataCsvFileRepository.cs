using System.Collections.Generic;
using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IMarketDataCsvFileRepository
    {
        IMarketDataEntities Load();
        void Save(IEnumerable<IMarketDataEntity> entities);
        void Update(IMarketDataEntities latestData);
    }
}