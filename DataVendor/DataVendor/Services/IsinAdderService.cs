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
            Console.Write("Market data are loading ...");
            var entities = _marketDataCsvFileRepository.Load();
            Console.Write($" {entities.Count} lines are loaded.\n");

            Console.Write("ISINs are loading ...");
            var isins = _isinsCsvFileRepository.Load();
            Console.Write($" {isins.Count} lines are loaded.\n");

            Console.Write("ISINs are being added ...");
            AddIsinToEntities(entities, isins);
            Console.Write(" done.\n");

            Console.Write("Market data are being saved ...");
            _marketDataCsvFileRepository.Save(entities);
            Console.Write(" done.\n");

            Console.Write("Unused ISINs are being removed ...");
            RemoveIsinFromIsins(isins, entities);
            Console.Write(" done.\n");

            Console.Write("ISINs data are being saved ...");
            _isinsCsvFileRepository.Save(isins);
            Console.Write(" done.\n");
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