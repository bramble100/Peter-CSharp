using NLog;
using Peter.Models.Interfaces;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataVendor.Services
{
    public class IsinAdderService : IIsinAdderService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMarketDataRepository _marketDataCsvFileRepository;
        private readonly IIsinsRepository _isinsCsvFileRepository;

        public IsinAdderService()
        {
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
            _isinsCsvFileRepository = new IsinsCsvFileRepository();
        }

        public IsinAdderService(IMarketDataRepository marketDataCsvFileRepository, IIsinsRepository isinsRepository) : this()
        {
            _marketDataCsvFileRepository = marketDataCsvFileRepository;
            _isinsCsvFileRepository = isinsRepository;
        }

        public void AddIsinsToEntities()
        {
            var entities = _marketDataCsvFileRepository.GetAll();

            var isins = _isinsCsvFileRepository.GetAll();

            AddIsinToEntities(entities, isins);

            _marketDataCsvFileRepository.SaveChanges();
            _logger.Info("Market data saved.");

            var removeCount = RemoveIsinFromIsins(isins, entities);
            _logger.Info($"{removeCount} ISIN(s) are removed.");

            var addCount = AddNewNames(isins, entities);
            _logger.Info($"{addCount} new name(s) are added.");

            _isinsCsvFileRepository.SaveChanges(isins);
            _logger.Info("ISINs saved.");
        }

        private static void AddIsinToEntities(IMarketDataEntities entities, INameToIsins isins) =>
            entities
                .Where(e => isins.ContainsKey(e.Name))
                .ToList()
                .ForEach(e => e.Isin = isins[e.Name]);

        private static int RemoveIsinFromIsins(INameToIsins isins, IMarketDataEntities entities)
        {
            var namesInEntities = entities
                .Select(e => e.Name)
                .Distinct();

            var deadNames = isins
                .Where(i => !namesInEntities.Contains(i.Key))
                .ToList();

            deadNames.ForEach(dn => isins.Remove(dn));

            return deadNames.Count;
        }

        private int AddNewNames(INameToIsins isins, IMarketDataEntities entities)
        {
            var namesInEntities = entities
                .Select(e => e.Name)
                .Distinct();

            var newNames = namesInEntities
                .Where(e => !isins.ContainsKey(e))
                .ToList();

            newNames.ForEach(n => isins.Add(new KeyValuePair<string, string>(n, string.Empty)));

            return newNames.Count;
        }
    }
}