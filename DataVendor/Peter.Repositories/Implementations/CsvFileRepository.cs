using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public abstract class CsvFileRepository
    {
        protected readonly string _baseDirectory;
        protected readonly string _fileNameExtension;
        protected readonly string _separator;

        protected string _backupDirectory;
        protected string _fileName;
        protected string _workingDirectory;

        protected string[] _header;

        private readonly string _dateFormat;

        /// <summary>
        /// The directory in which the provider works.
        /// </summary>
        protected string WorkingDirectory
        {
            get => _workingDirectory;
            set
            {
                if (Directory.Exists(value))
                {
                    _workingDirectory = value;
                    return;
                }

                try
                {
                    Directory.CreateDirectory(value);
                    _workingDirectory = value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Directory ({value}) cannot be created. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// The directory in which the provider saves backups.
        /// </summary>
        protected string BackupDirectory
        {
            get => _backupDirectory;
            set
            {
                if (Directory.Exists(value))
                {
                    _backupDirectory = value;
                    return;
                }

                try
                {
                    Directory.CreateDirectory(value);
                    _backupDirectory = value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Directory ({value}) cannot be created. {ex.Message}");
                }
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

            _backupDirectory = reader.GetValue("BackupDirectory", typeof(string)).ToString();
            BackupDirectory = Path.Combine(_workingDirectory, _backupDirectory);

            _separator = reader.GetValue("CsvSeparator", typeof(string)).ToString();
            _dateFormat = reader.GetValue("DateFormatForFileName", typeof(string)).ToString();
            _fileNameExtension = reader.GetValue("CsvFileNameExtension", typeof(string)).ToString();
        }

        protected static List<string> AddHeader(string[] header, string separator) => new List<string> { string.Join(separator, header) };

        protected static void RemoveHeader(TextFieldParser parser) => parser.ReadLine();

        protected Tuple<string, CultureInfo> GetCsvSeparatorAndCultureInfo(string header)
        {
            if (header.Contains(",") && !header.Contains(";"))
                return new Tuple<string, CultureInfo>(",", new CultureInfo("us-EN"));
            else if ((header.Contains(";") || header.Contains("\t")) && !header.Contains(","))
                return new Tuple<string, CultureInfo>(";", new CultureInfo("hu-HU"));
            throw new ArgumentOutOfRangeException(nameof(header), "Separator and CultureInfo cannot be determined.");
        }

        protected void CreateBackUp(string workingDir, string backupDir, string fileName)
        {
            try
            {
                File.Move(
                    Path.Combine(workingDir, fileName),
                    Path.Combine(
                        backupDir,
                        $"{Path.GetFileNameWithoutExtension(fileName)} {DateTime.Now.ToString(_dateFormat)}{Path.GetExtension(fileName)}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Backup ({fileName}) cannot be created. {ex.Message}");
            }
        }

        protected static void SaveActualFile(string workingDir, string fileName, IEnumerable<string> content)
        {
            try
            {
                File.WriteAllLines(Path.Combine(workingDir, fileName), content, Encoding.UTF8);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        internal static void SaveChanges(string[] header, IEnumerable<string> content, string fileName, string separator)
        {
            List<string> strings = AddHeader(header, separator);

            strings.AddRange(content);

            File.WriteAllLines(
                fileName,
                strings,
                Encoding.UTF8);
        }
    }
}
