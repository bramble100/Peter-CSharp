using DataVendor.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Text;

namespace DataVendor.Repositories
{
    public class IsinsCsvFileRepository
    {
        private const string separator = ",";
        private const string _fileNameExtension = "csv";
        private string _workingDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "StockExchange");

        private readonly Isins _isins = new Isins();

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
        internal void Load()
        {
            var filePath = Path.Combine(_workingDirectory, "isins.csv");
            using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.SetDelimiters(separator);

                RemoveHeader(parser);

                while (!parser.EndOfData)
                {
                    _isins.Add(parser.ReadFields());
                }
            }
        }

        private static void RemoveHeader(TextFieldParser parser) => parser.ReadLine();
    }

}
