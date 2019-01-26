using System.Collections.Generic;
using System.Linq;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;

namespace RegistryManager.Services
{
    public class RegistryService : IRegistryService
    {
        private readonly IMarketDataRepository _marketDataRepository;
        private readonly IRegistryRepository _registryRepository;

        public RegistryService()
        {
            _marketDataRepository = new MarketDataCsvFileRepository();
            _registryRepository = new RegistryCsvFileRepository();
        }

        public RegistryService(IMarketDataRepository marketDataRepository, IRegistryRepository registryRepository)
        {
            _marketDataRepository = marketDataRepository;
            _registryRepository = registryRepository;
        }

        public void Update()
        {
            var marketDataEntries = _marketDataRepository.GetAll();

            _registryRepository.AddRange(GetNewRegistryEntries(marketDataEntries));
            _registryRepository.RemoveRange(GetOutdatedRegistryEntries(marketDataEntries));

            _registryRepository.SaveChanges();
        }

        private IEnumerable<KeyValuePair<string, IRegistryEntry>> GetNewRegistryEntries(IMarketDataEntities marketDataEntries)
        {
            var isinsFromMarketData = marketDataEntries.Where(e => !string.IsNullOrWhiteSpace(e.Isin)).Select(e => e.Isin).ToHashSet();
            var isinsFromRegistry = _registryRepository.Isins.ToHashSet();
            var newIsins = isinsFromMarketData.Except(isinsFromRegistry);

            return newIsins
                .Select(isin => new KeyValuePair<string, IRegistryEntry>(
                    isin,
                    new RegistryEntry(
                        marketDataEntries
                        .First(d => string.Equals(isin, d.Isin))
                        .Name)));
        }

        private IEnumerable<string> GetOutdatedRegistryEntries(IMarketDataEntities marketDataEntries)
        {
            var isinsFromMarketData = marketDataEntries
                .Where(e => !string.IsNullOrWhiteSpace(e.Isin))
                .Select(e => e.Isin)
                .ToHashSet();
            var isinsFromRegistry = _registryRepository.Isins.ToHashSet();

            return isinsFromRegistry.Except(isinsFromMarketData);
        }
    }
}
