using NLog;
using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataVendor.Services
{
    public class IsinAdderService : IIsinAdderService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IIsinsRepository _isinsCsvFileRepository;
        private readonly IMarketDataRepository _marketDataCsvFileRepository;

        public IsinAdderService(
            IIsinsRepository isinsRepository,
            IMarketDataRepository marketDataCsvFileRepository)
        {
            _isinsCsvFileRepository = isinsRepository;
            _marketDataCsvFileRepository = marketDataCsvFileRepository;
        }

        public void AddIsinsToEntities()
        {
            var entities = _marketDataCsvFileRepository.GetAll();

            AddIsinToEntities(entities);

            _marketDataCsvFileRepository.SaveChanges();
            _logger.Info("Market data saved.");

            var removeCount = RemoveIsinFromIsins(entities);
            _logger.Info($"{removeCount} ISIN(s) are removed.");

            var addCount = AddNewNames(entities);
            _logger.Info($"{addCount} new name(s) are added.");

            _isinsCsvFileRepository.SaveChanges();
            _logger.Info("ISINs saved.");
        }

        private void AddIsinToEntities(IEnumerable<IMarketDataEntity> entities) =>
            entities
                .Where(e => _isinsCsvFileRepository.ContainsName(e.Name))
                .ToList()
                .ForEach(e => e.Isin = _isinsCsvFileRepository.GetIsinByCompanyName(e.Name));

        private int RemoveIsinFromIsins(IEnumerable<IMarketDataEntity> entities)
        {
            var namesInEntities = entities
                .Select(e => e.Name)
                .Distinct();

            var deadNames = _isinsCsvFileRepository.GetNames().Except(namesInEntities);

            deadNames.ToList().ForEach(name => _isinsCsvFileRepository.Remove(name));

            return deadNames.Count;
        }

        private int AddNewNames(IEnumerable<IMarketDataEntity> entities)
        {
            var namesInEntities = entities
                .Select(e => e.Name)
                .Distinct();

            var newNames = namesInEntities
                .Where(e => !_isinsCsvFileRepository.ContainsName(e))
                .ToList();

            newNames.ForEach(n => _isinsCsvFileRepository.Add(n));

            return newNames.Count;
        }
    }
}