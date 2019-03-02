using NLog;
using Peter.Repositories.Interfaces;
using System.Linq;

namespace Services.DataVendor
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

            RemoveDeadIsins();            
            AddNewNames();

            _isinsCsvFileRepository.SaveChanges();
            _logger.Info("ISINs saved.");
        }

        private void AddIsinToEntities()
        {
            _logger.Info("Adding ISINs to market data ...");

            foreach (var entity in _marketDataCsvFileRepository.Entities)
            {
                _marketDataCsvFileRepository.UpdateEntityWithIsin(
                    entity,
                    _isinsCsvFileRepository.GetIsinByCompanyName(entity.Name));
            }

            _logger.Info("ISINs added to market data.");
        }

        private void RemoveDeadIsins()
        {
            _logger.Info("Removing discontinued ISIN(s) ...");

            var namesInEntities = _marketDataCsvFileRepository
                .Entities
                .Select(e => e.Name)
                .Distinct();

            var deadNames = _isinsCsvFileRepository.GetNames().Except(namesInEntities).ToArray();

            if (deadNames.Any())
            {
                foreach (var name in deadNames)
                {
                    _isinsCsvFileRepository.Remove(name);
                }
                _logger.Info($"{deadNames.Count()} ISIN(s) are removed.");
            }
            else
            {
                _logger.Info($"No ISINs were removed.");
            }
        }

        private void AddNewNames()
        {
            _logger.Info("Adding new names to ISIN list ...");

            var namesInEntities = _marketDataCsvFileRepository
                .Entities
                .Select(e => e.Name)
                .Distinct();

            var newNames = namesInEntities
                .Where(e => !_isinsCsvFileRepository.ContainsName(e))
                .ToList();

            if (newNames.Any())
            {
                foreach (var name in newNames)
                {
                    _isinsCsvFileRepository.Add(name);
                }
                _logger.Info($"{newNames.Count} new name(s) are added.");
            }
            else
            {
                _logger.Info($"No names were removed.");
            }
        }
    }
}