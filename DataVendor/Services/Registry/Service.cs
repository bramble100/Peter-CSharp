using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NLog;
using Peter.Models.Builders;
using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;

namespace Services.Registry
{
    public class Service : IService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMarketDataRepository _marketDataRepository;
        private readonly IRegistryRepository _registryRepository;

        public Service(
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
            var outdatedIsins = _registryRepository.Isins.Except(_marketDataRepository.Isins).ToImmutableList();
            if (!outdatedIsins.Any())
            {
                _logger.Info("No entry to remove.");
                return;
            }

            _logger.Info($"Removing {outdatedIsins.Count} entry(s) ...");
            _registryRepository.RemoveRange(outdatedIsins);
            _logger.Info($"{outdatedIsins.Count} entry(s) removed.");
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
            string name;

            var isinsAndNamesInMarketData = (from entity in _marketDataRepository.Entities
                                             select new { entity.Isin, entity.Name })
                                             .Distinct()
                                             .ToImmutableArray();

            var newIsins = _marketDataRepository
                .Isins
                .Except(_registryRepository.Isins)
                .ToImmutableList();

            foreach (var isin in newIsins)
            {
                name = isinsAndNamesInMarketData.Single(d => string.Equals(isin, d.Isin)).Name;
                yield return new RegistryEntryBuilder()
                    .SetName(name)
                    .SetIsin(isin)
                    .Build();
            }
        }
    }
}
