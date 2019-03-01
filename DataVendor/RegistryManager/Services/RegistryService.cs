using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NLog;
using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;

namespace RegistryManager.Services
{
    public class RegistryService : IRegistryService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMarketDataRepository _marketDataRepository;
        private readonly IRegistryRepository _registryRepository;

        public RegistryService(
            IMarketDataRepository marketDataRepository,
            IRegistryRepository registryRepository)
        {
            _marketDataRepository = marketDataRepository;
            _registryRepository = registryRepository;
        }

        public void Update()
        {
            RemoveOutDatedEntries();
            AddNewEntries();

            _registryRepository.SaveChanges();
        }

        private void RemoveOutDatedEntries()
        {
            _logger.Info($"Removing outdated entries ...");
            var isins = _registryRepository.Isins.Except(_marketDataRepository.Isins).ToImmutableList();
            if (!isins.Any())
            {
                _logger.Info("No entry to remove.");
                return;
            }

            _logger.Info($"Removing {isins.Count} entry(s) ...");
            _registryRepository.RemoveRange(isins);
            _logger.Info($"{isins.Count} entry(s) removed.");
        }

        private void AddNewEntries()
        {
            _logger.Info($"Adding new entries ...");
            var newEntries = GetNewRegistryEntries().ToImmutableList();

            if (!newEntries.Any())
            {
                _logger.Info("No entry to add.");
                return;
            }

            _logger.Info($"Adding {newEntries.Count} entry(s) ...");
            _registryRepository.AddRange(newEntries);
            _logger.Info($"{newEntries.Count} entry(s) added.");
        }

        private IEnumerable<IRegistryEntry> GetNewRegistryEntries()
        {
            var isinsInMarketData = _marketDataRepository.Isins;
            var newIsins = isinsInMarketData.Except(_registryRepository.Isins).ToImmutableList();
            var newEntries = newIsins
                .Select(isin => _registryRepository.GetById(isin))
                .ToImmutableList();
            return newEntries;
        }
    }
}
