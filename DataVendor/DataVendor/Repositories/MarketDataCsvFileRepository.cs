using DataVendor.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataVendor.Repositories
{
    public class MarketDataCsvFileRepository
    {
        protected const string _dateFormat = "yyyy-MM-dd-HH-mm";
        private const string separator = ",";
        private const string _fileNameExtension = "csv";
        private string _workingDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "StockExchange",
            "RawDownload");

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
                parser.SetDelimiters(separator);

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
            Save(entities);
        }

        /// <summary>
        /// Saves the entity collection into CSV file.
        /// </summary>
        /// <param name="entities"></param>
        internal void Save(IEnumerable<MarketDataEntity> entities)
        {
            List<string> strings = AddHeader();

            strings.AddRange(entities.Select(e => e.FormatterForCSV(separator)));

            File.WriteAllLines(
                Path.Combine(WorkingDirectory, FileNameCreator(entities.Max(e => e.DateTime))),
                strings,
                Encoding.UTF8);
        }

        private List<string> AddHeader() => new List<string> { String.Join(separator, header) };

        private static string FileNameCreator(DateTime dateTime) =>
            $"{dateTime.ToString(_dateFormat)}.{_fileNameExtension}";

        protected string FileNameCreator(DateTime dateTime, string stockExchangeName, string fileNameExtension) =>
            $"{dateTime.ToString(_dateFormat)} {stockExchangeName}.{fileNameExtension}";
    }
}