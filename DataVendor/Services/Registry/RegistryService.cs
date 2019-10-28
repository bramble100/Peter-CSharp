using Models.Builders;
using Models.Interfaces;
using NLog;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Services.Registry
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
            var outdatedIsins = _registryRepository.Isins.Except(_marketDataRepository.Isins).ToImmutableArray();

            if (outdatedIsins.Any())
            {
                _logger.Info($"Removing {outdatedIsins.Length} entry(s) ...");
                _registryRepository.RemoveRange(outdatedIsins);
                _logger.Info($"{outdatedIsins.Length} entry(s) removed.");
            }
            else
            {
                _logger.Info("No entry to remove.");
            }
        }

        private void AddNewEntries()
        {
            _logger.Info($"Adding new entries ...");
            var newEntries = GetNewRegistryEntries().ToImmutableList();

            if (newEntries.Any())
            {
                _logger.Info($"Adding {newEntries.Count} entry(s) ...");
                _registryRepository.AddRange(newEntries);
                _logger.Info($"{newEntries.Count} entry(s) added.");
            }
            else
            {
                _logger.Info("No entry to add.");
            }
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
                name = string.Empty;

                try
                {
                    name = isinsAndNamesInMarketData.Single(d => string.Equals(isin, d.Isin)).Name;
                }
                catch (InvalidOperationException ex)
                {               
                    _logger.Warn(ex, $"ISIN can be found more than once: {isin}.");
                }

                if(!string.IsNullOrWhiteSpace(name))
                {
                    yield return new RegistryEntryBuilder()
                        .SetName(name)
                        .SetIsin(isin)
                        .Build();
                }
            }
        }
    }
}
