using Microsoft.VisualBasic.FileIO;
using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class MarketDataCsvFileRepository : CsvFileRepository, IMarketDataCsvFileRepository
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

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
            var filePath = Directory.GetFiles(WorkingDirectory).Max();

            IMarketDataEntities entities = new MarketDataEntities();

            if (string.IsNullOrWhiteSpace(filePath))
                return entities;

            using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.SetDelimiters(_separator);

                RemoveHeader(parser);

                while (!parser.EndOfData)
                {
                    entities.Add(parser.ReadFields().ParserFromCSV());
                }
            }
            _logger.Info($"{entities.Count} new market data entities loaded.");
            return entities;
        }

        public void Update(IMarketDataEntities latestData)
        {
            _entities.AddRange(latestData);
            _logger.Info($"{latestData.Count} new market data entities added.");
            SaveChanges();
        }

        public void SaveChanges()
        {
            // TODO handle return bool
            // TODO handle return message
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