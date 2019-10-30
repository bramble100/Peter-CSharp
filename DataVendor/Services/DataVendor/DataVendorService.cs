using Models.Interfaces;
using NLog;
using Repositories.Exceptions;
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
        }

        public bool HaveAllMarketDataNamesIsins
        {
            get
            {
                var names = _marketDataRepository.GetNames();
                if (names.Any() && !_isinRepository.GetIsins().Any())
                {
                    return false;
                }

                bool result = true;

                foreach (var name in names)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(_isinRepository.FindIsinByName(name)))
                        {
                            result = false;
                        }
                    }
                    catch (RepositoryException ex)
                    {
                        result = false;
                        _logger.Error(ex.Message);
                    }
                }

                return result;
            }
        }

        public IEnumerable<string> FindIsinsWithLatestMarketData()
        {
            var data = _marketDataRepository.GetAll().ToArray();

            if (!data.Any())
            {
                yield break;
            }

            var names = data
                .Where(d => d.DateTime.Date == data.Max(d2 => d2.DateTime).Date)
                .Select(d => d.Name)
                .Distinct()
                .ToArray();

            foreach (var name in names)
            {
                if (_isinRepository.ContainsName(name))
                {
                    yield return _isinRepository.FindIsinByName(name);
                }
                else
                {
                    _isinRepository.Add(name);
                }
            }

            _isinRepository.SaveChanges();
        }

        public IEnumerable<IMarketDataEntity> FindMarketDataByIsin(string isin)
            => _marketDataRepository.FindByNames(_isinRepository.FindNamesByIsin(isin));

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