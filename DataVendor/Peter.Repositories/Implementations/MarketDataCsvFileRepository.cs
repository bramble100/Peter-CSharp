using Microsoft.VisualBasic.FileIO;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class MarketDataCsvFileRepository : CsvFileRepository, IMarketDataCsvFileRepository
    {
        public MarketDataCsvFileRepository() : base()
        {
            var reader = new AppSettingsReader();

            var rawDownloadsDirectory = reader.GetValue("WorkingDirectoryRawDownloads", typeof(string)).ToString();

            WorkingDirectory = Path.Combine(_workingDirectory, rawDownloadsDirectory);

            _fileName = reader.GetValue("MarketDataFileName", typeof(string)).ToString();
        }

        public IMarketDataEntities Load()
        {
            var filePath = Directory.GetFiles(_workingDirectory).Max();

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
            return entities;
        }

        public void Update(IMarketDataEntities latestData)
        {
            var entities = Load();
            entities.AddRange(latestData);
            entities.Sort();
            Save(entities);
        }

        public void Save(IEnumerable<IMarketDataEntity> entities)
        {
            CreateBackUp(
                WorkingDirectory, 
                BackupDirectory, 
                _fileName);
            SaveChanges(
                CsvLineMarketData.Header,
                entities.Select(e => e.FormatterForCSV(_separator)),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }
    }
}