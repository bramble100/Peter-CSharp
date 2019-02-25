using NLog;
using Peter.Repositories.Interfaces;
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

        public void AddIsinsToMarketData()
        {
            AddIsinToEntities();

            _marketDataCsvFileRepository.SaveChanges();
            _logger.Info("Market data saved.");

            var removeCount = RemoveDeadIsins();
            _logger.Info($"{removeCount} ISIN(s) are removed.");

            var addCount = AddNewNames();
            _logger.Info($"{addCount} new name(s) are added.");

            _isinsCsvFileRepository.SaveChanges();
            _logger.Info("ISINs saved.");
        }

        private void AddIsinToEntities()
        {
            foreach (var entity in _marketDataCsvFileRepository.Entities)
            {
                _marketDataCsvFileRepository.UpdateEntityWithIsin(
                    entity,
                    _isinsCsvFileRepository.GetIsinByCompanyName(entity.Name));
            }
        }

        private int RemoveDeadIsins()
        {
            var namesInEntities = _marketDataCsvFileRepository
                .Entities
                .Select(e => e.Name)
                .Distinct();

            var deadNames = _isinsCsvFileRepository.GetNames().Except(namesInEntities);

            deadNames.ToList().ForEach(name => _isinsCsvFileRepository.Remove(name));

            return deadNames.Count;
        }

        private int AddNewNames()
        {
            var namesInEntities = _marketDataCsvFileRepository
                .Entities
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