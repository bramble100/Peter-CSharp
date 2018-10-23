using Microsoft.VisualBasic.FileIO;
using NLog;
using Peter.Repositories.Exceptions;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace Peter.Repositories.Implementations
{
    public abstract class CsvFileRepository
    {
        protected readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        protected readonly string _baseDirectory;
        protected readonly string _fileNameExtension;
        protected readonly string _separator;

        protected string _backupDirectory;
        protected string _fileName;
        protected string _workingDirectory;

        private readonly string _dateFormat;
        private readonly IFileSystemFacade _fileSystemFacade;

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
                    _logger.Error($"Working directory ({value}) does not exist.");
                    throw new RepositoryException($"Working directory ({value}) does not exist.");
                }
                _workingDirectory = value;
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
                    _logger.Error(ex, $"Backup directory ({value}) cannot be created. {ex.Message}");
                    throw new RepositoryException($"Backup directory ({value}) cannot be created. {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CsvFileRepository()
        {
            _fileSystemFacade = new FileSystemFacade();

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

        internal void SaveChanges(string[] header, IEnumerable<string> content, string fullPath, string separator)
        {
            try
            {
                List<string> contentWithHeader = AddHeader(header, separator);
                contentWithHeader.AddRange(content);
                var stringContent = string.Join("\n", contentWithHeader);

                _fileSystemFacade.Save(fullPath, stringContent);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error when saving changes. {ex.Message}");
                throw new RepositoryException($"Error when saving changes.", ex);
            }
        }

        protected static List<string> AddHeader(string[] header, string separator) => new List<string> { string.Join(separator, header) };

        protected static void RemoveHeader(TextFieldParser parser) => parser.ReadLine();

        protected Tuple<string, CultureInfo> GetCsvSeparatorAndCultureInfo(string header)
        {
            if (header.Contains(","))
                return new Tuple<string, CultureInfo>(",", new CultureInfo("us-EN"));
            else if (header.Contains(";"))
                return new Tuple<string, CultureInfo>(";", new CultureInfo("hu-HU"));
            else if (header.Contains("\t"))
                return new Tuple<string, CultureInfo>("\t", new CultureInfo("hu-HU"));
            throw new ArgumentOutOfRangeException(nameof(header), "Separator and CultureInfo cannot be determined.");
        }

        protected void CreateBackUp(string workingDir, string backupDir, string fileName)
        {
            try
            {
                _fileSystemFacade.Backup(
                    Path.Combine(workingDir, fileName),
                    Path.Combine(
                        backupDir,
                        $"{Path.GetFileNameWithoutExtension(fileName)} {DateTime.Now.ToString(_dateFormat)}{Path.GetExtension(fileName)}"));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error when saving changes. {ex.Message}");
                throw new RepositoryException($"Error when creating backup.", ex);
            }
        }
    }
}
