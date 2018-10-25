using Microsoft.VisualBasic.FileIO;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Exceptions;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class MarketDataCsvFileRepository : CsvFileRepository, IMarketDataCsvFileRepository
    {
        private readonly IMarketDataEntities _entities;

        public MarketDataCsvFileRepository() : base()
        {
            var reader = new AppSettingsReader();
            _fileName = reader.GetValue("MarketDataFileName", typeof(string)).ToString();
            WorkingDirectory = Path.Combine(
                _workingDirectory,
                reader.GetValue("WorkingDirectoryRawDownloads", typeof(string)).ToString());

            _entities = Load();
        }

        public IMarketDataEntities Load()
        {
            try
            {
                Tuple<string, CultureInfo> baseInfo;
                var filePath = Path.Combine(_workingDirectory, _fileName);

                baseInfo = GetCsvSeparatorAndCultureInfo(
                    File.ReadLines(filePath, Encoding.UTF8).FirstOrDefault());

                IMarketDataEntities entities = new MarketDataEntities();

                if (string.IsNullOrWhiteSpace(filePath))
                    return entities;

                using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
                {
                    parser.SetDelimiters(baseInfo.Item1);

                    RemoveHeader(parser);

                    while (!parser.EndOfData)
                    {
                        // TODO replace extension
                        entities.Add(parser.ReadFields().ParserFromCSV());
                    }
                }
                _logger.Info($"{entities.Count} new market data entities loaded.");
                return entities;
            }
            catch (System.Exception ex)
            {
                _logger.Error($"Error when loading entities in {GetType().Name}.");
                throw new RepositoryException($"Error when loading entities in {GetType().Name}.", ex);
            }
        }

        public void Update(IMarketDataEntities latestData)
        {
            _entities.AddRange(latestData);
            _logger.Info($"{latestData.Count} new market data entities added.");
            SaveChanges();
        }

        public void SaveChanges()
        {
            CreateBackUp(
                WorkingDirectory,
                BackupDirectory,
                _fileName);

            _entities.Sort();

            SaveChanges(
                CsvLineMarketData.Header,
                // TODO use CsvLineMarketData for CSV formatting
                _entities.Select(e => e.FormatterForCSV(_separator)),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }

        public IMarketDataEntities GetAll() => _entities;
    }
}