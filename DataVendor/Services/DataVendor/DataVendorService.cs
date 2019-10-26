using Models.Interfaces;
using NLog;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Services.DataVendor
{
    /// <summary>
    /// Manages all market data together with ISINs.
    /// </summary>
    public class DataVendorService : IDataVendorService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IIsinsRepository _isinRepository;
        private readonly IMarketDataRepository _marketDataRepository;

        public DataVendorService(
            IIsinsRepository isinsRepository,
            IMarketDataRepository marketDataCsvFileRepository)
        {
            _isinRepository = isinsRepository;
            _marketDataRepository = marketDataCsvFileRepository;

            RepairIsinRepository();
        }

        public IEnumerable<string> FindIsinsWithLatestMarketData()
        {
            throw new System.NotImplementedException();
        }

        public  IEnumerable<IMarketDataEntity> FindMarketDataByIsin(string isin) 
            => _marketDataRepository.FindByNames(_isinRepository.FindNamesByIsin(isin));

        public IEnumerable<KeyValuePair<string, IEnumerable<IMarketDataEntity>>> GetMarketDataGroupedByIsin()
        {
            foreach (var isin in _isinRepository.GetIsins())
            {
                yield return new KeyValuePair<string, IEnumerable<IMarketDataEntity>>(isin, FindMarketDataByIsin(isin));
            }
        }

        internal void AddNewNames()
        {
            _logger.Debug("Adding new names ...");

            var missingNamesFromIsinRepo = _marketDataRepository
                .GetNames()
                .Except(_isinRepository.GetNames())
                .Distinct()
                .ToList();

            if (missingNamesFromIsinRepo.Any())
            {
                missingNamesFromIsinRepo
                    .ForEach(name => _logger.Warn($"{name} to be added to ISIN-Name converter."));
                _isinRepository.AddRange(missingNamesFromIsinRepo);
                _logger.Info($"{missingNamesFromIsinRepo.Count} new name(s) were added.");
                _logger.Info($"Remember to add ISIN to newly added name(s)!");
            }
            else
            {
                _logger.Info($"No names were added.");
            }
        }

        private void RemoveDeadNames()
        {
            _logger.Debug("Removing discontinued ISIN(s) ...");

            var missingNamesFromMarketDataRepo = _isinRepository
                .GetNames()
                .Except(_marketDataRepository.GetNames())
                .Distinct()
                .ToList();

            if (missingNamesFromMarketDataRepo.Any())
            {
                missingNamesFromMarketDataRepo
                    .ForEach(name => _logger.Warn($"{name} to be removed from ISIN-Name converter."));
                _isinRepository.RemoveRange(missingNamesFromMarketDataRepo);
                _logger.Debug($"{missingNamesFromMarketDataRepo.Count} old name(s) were removed.");
            }
            else
            {
                _logger.Info($"No ISINs/names were removed.");
            }
        }

        private void RepairIsinRepository()
        {
            _logger.Info("Repairing datavendor ISIN repository ...");

            AddNewNames();
            RemoveDeadNames();

            _isinRepository.SaveChanges();
        }
    }
}