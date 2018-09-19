using DataVendor.Models;
using DataVendor.Repositories;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;
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
            var count = RemoveIsinFromIsins(isins, entities);
            Console.Write($" {count} ISIN(s) are removed.\n");

            Console.Write("New names are being added ...");
            count = AddNewNames(isins, entities);
            Console.Write($" {count} name(s) are added.\n");

            Console.Write("ISINs data are being saved ...");
            _isinsCsvFileRepository.Save(isins);
            Console.Write(" done.\n");
        }

        private static void AddIsinToEntities(IMarketDataEntities entities, IIsins isins)
        {
            entities
                .Where(e => isins.ContainsKey(e.Name))
                .ToList()
                .ForEach(e => e.Isin = isins[e.Name]);
        }

        private int RemoveIsinFromIsins(IIsins isins, IMarketDataEntities entities)
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

        private int AddNewNames(IIsins isins, IMarketDataEntities entities)
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