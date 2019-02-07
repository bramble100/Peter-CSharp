using Infrastructure;
using Microsoft.VisualBasic.FileIO;
using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Exceptions;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class MarketDataCsvFileRepository : CsvFileRepository, IMarketDataRepository
    {
        protected new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMarketDataEntities _entities;

        public MarketDataCsvFileRepository(
            IConfig config, 
            IFileSystemFacade fileSystemFacade) 
            : base(config, fileSystemFacade)
        {
            WorkingDirectory = Path.Combine(
                WorkingDirectory,
                _config.GetValue<string>("WorkingDirectoryRawDownloads"));
            _logger.Debug($"Working directory is {WorkingDirectory} from config file.");

            _fileName = _config.GetValue<string>("MarketDataFileName");
            _logger.Debug($"Market data filename is {_fileName} from config file.");

            _entities = new MarketDataEntities();
        }

        public IEnumerable<string> Isins
        {
            get
            {
                if (!_fileContentLoaded) Load();

                return _entities.Isins;
            }
        }

        public void AddRange(IMarketDataEntities latestData)
        {
            if (!_fileContentLoaded) Load();

            _logger.Info("Adding market data entities ...");
            _entities.AddRange(latestData);
            _logger.Info($"{latestData.Count} new market data entities added.");
        }

        public IMarketDataEntities GetAll()
        {
            if (!_fileContentLoaded) Load();

            return _entities;
        }

        public void SaveChanges()
        {
            if (!_fileContentLoaded) return;

            CreateBackUp(
                WorkingDirectory,
                BackupDirectory,
                _fileName);

            _entities.Sort();

            SaveChanges(
                CsvLineMarketData.Header,
                _entities.Select(e => CsvLineMarketData.FormatForCSV(e, _separator, _cultureInfo)),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }

        private void Load()
        {
            try
            {
                Tuple<string, CultureInfo> baseInfo;
                var fullPath = Path.Combine(WorkingDirectory, _fileName);

                baseInfo = GetCsvSeparatorAndCultureInfo(
                    _fileSystemFacade.ReadLines(fullPath, Encoding.UTF8).FirstOrDefault());

                _logger.Debug($"{_fileName}: separator: \"{baseInfo.Item1}\" culture: \"{baseInfo.Item2.ToString()}\".");

                _logger.Info("Loading market data entities from CSV file ...");

                using (var parser = new TextFieldParser(fullPath, Encoding.UTF8))
                {
                    parser.SetDelimiters(baseInfo.Item1);

                    RemoveHeader(parser);

                    while (!parser.EndOfData)
                    {
                        if (CsvLineMarketData.TryParseFromCsv(
                            parser.ReadFields(),
                            baseInfo.Item2,
                            out IMarketDataEntity result))
                        {
                            _entities.Add(result);
                        }
                    }
                }

                _fileContentLoaded = true;
                _logger.Info($"{_entities.Count} new market data entities loaded.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error when loading entities in {GetType().Name}.");
                throw new RepositoryException($"Error when loading entities in {GetType().Name}.", ex);
            }
        }
    }
}