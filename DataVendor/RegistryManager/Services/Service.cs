using System;
using System.Collections.Generic;
using System.Linq;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;

namespace RegistryManager.Services
{
    public class Service
    {
        private readonly IRegistryCsvFileRepository _registryCsvFileRepository;
        private readonly IMarketDataCsvFileRepository _marketDataCsvFileRepository;

        public Service()
        {
            _registryCsvFileRepository = new RegistryCsvFileRepository();
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
        }

        internal void Update()
        {
            var marketDataEntries = _marketDataCsvFileRepository.Load();

            _registryCsvFileRepository.AddRange(GetNewRegistryEntries(_registryCsvFileRepository.Entities, marketDataEntries));
            //RemoveOldRegistryEntries();

            _registryCsvFileRepository.SaveChanges();
        }

        private IEnumerable<KeyValuePair<string, IRegistryEntry>> GetNewRegistryEntries(IRegistry registryEntries, IMarketDataEntities marketDataEntries)
        {
            var isinsFromMarketData = marketDataEntries.Where(e => !string.IsNullOrWhiteSpace(e.Isin)).Select(e => e.Isin).ToHashSet();
            var isinsFromRegistry = registryEntries.Select(e => e.Key).ToHashSet();
            var newIsins = isinsFromMarketData.Except(isinsFromRegistry);

            return newIsins
                .Select(isin => new KeyValuePair<string, IRegistryEntry>(
                    isin,
                    new RegistryEntry(
                        marketDataEntries
                        .First(d => string.Equals(isin, d.Isin)).Name)));
        }

        private void RemoveOldRegistryEntries()
        {
            throw new NotImplementedException();
        }
    }
}
