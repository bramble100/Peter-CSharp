using DataVendor.Repositories.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataVendor.Services
{
    public class IsinAdderService
    {
        private readonly IMarketDataCsvFileRepository _marketDataCsvFileRepository;
        private readonly IIsinsCsvFileRepository _isinsCsvFileRepository;

        public IsinAdderService()
        {
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
            _isinsCsvFileRepository = new IsinsCsvFileRepository();
        }

        public void AddIsinsToEntities()
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
            var count = RemoveIsinFromIsins(isins, entities);
            Console.Write($" {count} ISIN(s) are removed.\n");

            Console.Write("New names are being added ...");
            count = AddNewNames(isins, entities);
            Console.Write($" {count} name(s) are added.\n");

            Console.Write("ISINs data are being saved ...");
            _isinsCsvFileRepository.Save(isins);
            Console.Write(" done.\n");
        }

        private static void AddIsinToEntities(IMarketDataEntities entities, INameToIsin isins)
        {
            entities
                .Where(e => isins.ContainsKey(e.Name))
                .ToList()
                .ForEach(e => e.Isin = isins[e.Name]);
        }

        private int RemoveIsinFromIsins(INameToIsin isins, IMarketDataEntities entities)
        {
            // TODO: refactor

            var namesInEntities = entities
                .Select(e => e.Name)
                .Distinct();

            var deadNames = isins
                .Where(i => !namesInEntities.Contains(i.Key))
                .ToList();

            deadNames                
                .ForEach(dn=> isins.Remove(dn));

            return deadNames.Count;
        }

        private int AddNewNames(INameToIsin isins, IMarketDataEntities entities)
        {
            var namesInEntities = entities
                .Select(e => e.Name)
                .Distinct();

            var newNames = namesInEntities
                .Where(e => !isins.ContainsKey(e))
                .ToList();

            newNames
                .ForEach(n => isins.Add(new KeyValuePair<string, string>(n, string.Empty)));

            return newNames.Count;
        }
    }
}