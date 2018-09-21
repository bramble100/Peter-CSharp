using System.Collections.Generic;
using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IRegistryCsvFileRepository
    {
        IMarketDataEntities Load();
        void Save(IEnumerable<IMarketDataEntity> entities);
        void Update(IMarketDataEntities latestData);
    }
}