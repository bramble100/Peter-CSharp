using NLog;
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
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMarketDataRepository _marketDataCsvFileRepository;
        private readonly IIsinsRepository _isinsCsvFileRepository;

        public IsinAdderService()
        {
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
            _isinsCsvFileRepository = new IsinsCsvFileRepository();
        }

        public void AddIsinsToEntities()
        {
            var entities = _marketDataCsvFileRepository.GetAll();

            var isins = _isinsCsvFileRepository.GetAll();

            AddIsinToEntities(entities, isins);

            _marketDataCsvFileRepository.SaveChanges();
            _logger.Info("Market data saved.");

            var count = RemoveIsinFromIsins(isins, entities);
            _logger.Info($"{count} ISIN(s) are removed.");

            count = AddNewNames(isins, entities);
            _logger.Info($"{count} new name(s) are added.");

            _isinsCsvFileRepository.SaveChanges(isins);
            _logger.Info("ISINs saved.");
        }

        private static void AddIsinToEntities(IMarketDataEntities entities, INameToIsin isins) => 
            entities
                .Where(e => isins.ContainsKey(e.Name))
                .ToList()
                .ForEach(e => e.Isin = isins[e.Name]);

        private int RemoveIsinFromIsins(INameToIsin isins, IMarketDataEntities entities)
        {
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