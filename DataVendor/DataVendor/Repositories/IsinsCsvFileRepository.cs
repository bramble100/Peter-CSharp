using DataVendor.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataVendor.Repositories
{
    public class IsinsCsvFileRepository
    {
        private const string _separator = ",";
        private const string _fileName = "isins.csv";
        private string _workingDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "StockExchange");

        private readonly string[] header = {
            "Name",
            "ISIN"
        };

        /// <summary>
        /// Constructor.
        /// </summary>
        public IsinsCsvFileRepository()
        {
        }

        /// <summary>
        /// The directory in which the provider works.
        /// </summary>
        public IsinsCsvFileRepository(string workingDirectory) : this()
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
        /// Loads the CSV file and stores its content.
        /// </summary>
        /// <returns></returns>
        internal Isins Load()
        {
            var filePath = Path.Combine(_workingDirectory, _fileName);
            var isins = new Isins();

            using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.SetDelimiters(_separator);

                RemoveHeader(parser);

                while (!parser.EndOfData)
                {
                    isins.Add(parser.ReadFields());
                }
            }

            return isins;
        }

        internal void Save(Isins isins)
        {
            List<string> strings = AddHeader();

            strings.AddRange(isins.Select(i => i.FormatterForCSV(_separator)));

            File.WriteAllLines(
                Path.Combine(WorkingDirectory, _fileName),
                strings,
                Encoding.UTF8);
        }

        private List<string> AddHeader() => new List<string> { string.Join(_separator, header) };

        private static void RemoveHeader(TextFieldParser parser) => parser.ReadLine();
    }
}
