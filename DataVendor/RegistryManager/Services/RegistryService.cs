using System.Collections.Generic;
using System.Linq;
using Peter.Models.Interfaces;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;

namespace RegistryManager.Services
{
    public class RegistryService : IRegistryService
    {
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

        private void RemoveOutDatedEntries() =>
            _registryRepository.RemoveRange(_registryRepository.Isins.Except(_marketDataRepository.Isins));

        private void AddNewEntries() => 
            _registryRepository.AddRange(GetNewRegistryEntries());

        private IEnumerable<IRegistryEntry> GetNewRegistryEntries() => 
            _marketDataRepository
                .Isins
                .Except(_registryRepository.Isins)
                .Select(isin => _registryRepository.GetById(isin));
    }
}
