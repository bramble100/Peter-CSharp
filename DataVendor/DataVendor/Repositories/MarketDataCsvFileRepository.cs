using DataVendor.Models;
using Microsoft.VisualBasic.FileIO;
using Peter.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace DataVendor.Repositories
{
    public class MarketDataCsvFileRepository
    {
        private readonly string _dateFormat;
        private readonly string _separator;
        private readonly string _fileNameExtension;
        private string _workingDirectory;

        private readonly string[] header = {
            "Name",
            "ISIN",
            "Closing Price",
            "DateTime",
            "Volumen",
            "Previous Day Closing Price",
            "Stock Exchange"
        };

        /// <summary>
        /// Constructor.
        /// </summary>
        public MarketDataCsvFileRepository()
        {
            var reader = new AppSettingsReader();

            _separator = reader.GetValue("CsvSeparator", typeof(string)).ToString();
            _fileNameExtension = reader.GetValue("CsvFileNameExtension", typeof(string)).ToString();
            _dateFormat = reader.GetValue("DateFormatForFileName", typeof(string)).ToString();

            var baseDirectory = reader.GetValue("WorkingDirectoryBase", typeof(string)).ToString();
            var workingDirectory = reader.GetValue("WorkingDirectory", typeof(string)).ToString();
            var rawDownloadsDirectory = reader.GetValue("WorkingDirectoryRawDownloads", typeof(string)).ToString();

            if (string.IsNullOrWhiteSpace(baseDirectory) || string.Equals(baseDirectory.ToLower(), "desktop"))
            {
                baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            if (string.IsNullOrWhiteSpace(workingDirectory))
            {
                rawDownloadsDirectory = "StockExchange";
            }
            if (string.IsNullOrWhiteSpace(rawDownloadsDirectory))
            {
                rawDownloadsDirectory = "RawDownload";
            }

            WorkingDirectory = Path.Combine(baseDirectory, workingDirectory, rawDownloadsDirectory);

            _dateFormat = reader.GetValue("DateFormatForFileName", typeof(string)).ToString();
            _fileNameExtension = reader.GetValue("CsvFileNameExtension", typeof(string)).ToString();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workingDirectory"></param>
        internal MarketDataCsvFileRepository(string workingDirectory) : this()
        {
            WorkingDirectory = workingDirectory;
        }

        /// <summary>
        /// The directory in which the provider works.
        /// </summary>
        internal string WorkingDirectory
        {
            get => _workingDirectory;
            set
            {
                if (!Directory.Exists(value))
                {
                    throw new Exception($"Invalid directory specified ({value})");
                }
                _workingDirectory = value;
            }
        }

        /// <summary>
        /// Loads one CSV file and returns its content.
        /// </summary>
        /// <returns></returns>
        internal MarketDataEntities Load()
        {
            var filePath = Directory.GetFiles(_workingDirectory).Max();

            var entities = new MarketDataEntities();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                return entities;
            }

            using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.SetDelimiters(_separator);

                RemoveHeader(parser);

                while (!parser.EndOfData)
                {
                    entities.Add(parser.ReadFields().ParserFromCSV());
                }
                return entities;
            }
        }

        private static void RemoveHeader(TextFieldParser parser) => parser.ReadLine();

        /// <summary>
        /// Adds new market data to the existing data.
        /// </summary>
        /// <param name="latestData"></param>
        public void Update(MarketDataEntities latestData)
        {
            var entities = Load();
            entities.AddRange(latestData);
            entities.Sort();
            Save(entities);
        }

        /// <summary>
        /// Saves the entity collection into CSV file.
        /// </summary>
        /// <param name="entities"></param>
        internal void Save(IEnumerable<MarketDataEntity> entities)
        {
            List<string> strings = AddHeader();

            strings.AddRange(entities.Select(e => e.FormatterForCSV(_separator)));

            File.WriteAllLines(
                Path.Combine(WorkingDirectory, FileNameCreator(entities.Max(e => e.DateTime))),
                strings,
                Encoding.UTF8);
        }

        private List<string> AddHeader() => new List<string> { string.Join(_separator, header) };

        private string FileNameCreator(DateTime dateTime) =>
            $"{dateTime.ToString(_dateFormat)}.{_fileNameExtension}";

        protected string FileNameCreator(DateTime dateTime, string stockExchangeName, string fileNameExtension) =>
            $"{dateTime.ToString(_dateFormat)} {stockExchangeName}.{fileNameExtension}";
    }
}