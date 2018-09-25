using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace Peter.Repositories.Implementations
{
    public abstract class CsvFileRepository
    {
        protected readonly string _separator;

        protected string _workingDirectory;
        protected readonly string _baseDirectory;
        protected string _fileName;
        protected readonly string _fileNameExtension;

        protected string[] _header;

        /// <summary>
        /// The directory in which the provider works.
        /// </summary>
        protected string WorkingDirectory
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
        /// Constructor.
        /// </summary>
        public CsvFileRepository()
        {
            var reader = new AppSettingsReader();

            _baseDirectory = reader.GetValue("WorkingDirectoryBase", typeof(string)).ToString();
            if (string.IsNullOrWhiteSpace(_baseDirectory) || string.Equals(_baseDirectory.ToLower(), "desktop"))
            {
                _baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            _workingDirectory = reader.GetValue("WorkingDirectory", typeof(string)).ToString();

            WorkingDirectory = Path.Combine(_baseDirectory, _workingDirectory);

            _separator = reader.GetValue("CsvSeparator", typeof(string)).ToString();
            _fileNameExtension = reader.GetValue("CsvFileNameExtension", typeof(string)).ToString();
        }

        protected List<string> AddHeader(string separator) => new List<string> { string.Join(separator, _header) };

        protected static void RemoveHeader(TextFieldParser parser) => parser.ReadLine();

        protected Tuple<string, CultureInfo> GetCsvSeparatorAndCultureInfo(string header)
        {
            if (header.Contains(",") && !header.Contains(";"))
                return new Tuple<string, CultureInfo>(",", new CultureInfo("us-EN"));
            else if(header.Contains(";") && !header.Contains(","))
                return new Tuple<string, CultureInfo>(";", new CultureInfo("hu-HU"));
            throw new ArgumentOutOfRangeException("Separator and CultureInfo cannot be determined.");
        }
    }
}
