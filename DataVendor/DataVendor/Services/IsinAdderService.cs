using DataVendor.Models;
using DataVendor.Repositories;
using System;
using System.Linq;

namespace DataVendor.Services
{
    internal class IsinAdderService
    {
        private readonly MarketDataCsvFileRepository _marketDataCsvFileRepository;
        private readonly IsinsCsvFileRepository _isinsCsvFileRepository;

        public IsinAdderService()
        {
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
            _isinsCsvFileRepository = new IsinsCsvFileRepository();
        }

        internal void AddIsinsToEntities()
        {
            var entities = _marketDataCsvFileRepository.Load();
            var isins = _isinsCsvFileRepository.Load();

            AddIsinToEntities(entities, isins);
            _marketDataCsvFileRepository.Save(entities);

            RemoveIsinFromIsins(isins, entities);
            _isinsCsvFileRepository.Save(isins);
        }

        private static void AddIsinToEntities(MarketDataEntities entities, Isins isins)
        {
            entities
                .Where(e => isins.ContainsKey(e.Name))
                .ToList()
                .ForEach(e => e.Isin = isins[e.Name]);
        }

        private void RemoveIsinFromIsins(Isins isins, MarketDataEntities entities)
        {
            // TODO: refactor

            var namesInEntities = entities
                .Select(e => e.Name)
                .Distinct();

            var deadNames = isins
                .Where(i => !namesInEntities.Contains(i.Key));

            deadNames
                .ToList()
                .ForEach(dn=> isins.Remove(dn));
        }
    }
}