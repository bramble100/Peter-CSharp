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

        protected readonly string _fileNameExtension;
        protected readonly string _separator;
        protected readonly CultureInfo _cultureInfo;

        protected string _fileName;

        private string _backupDirectory;
        private string _baseDirectory;
        private string _workingDirectory;

        private readonly string _dateFormat;
        private readonly IFileSystemFacade _fileSystemFacade;

        /// <summary>
        /// The directory in which the provider saves backups.
        /// </summary>
        protected string BackupDirectory
        {
            get => _backupDirectory;
            set
            {
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
        /// The root directory in which the provider saves everything.
        /// </summary>
        protected string BaseDirectory
        {
            get => _baseDirectory;
            set
            {
                try
                {
                    Directory.CreateDirectory(value);
                    _baseDirectory = value;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Base directory ({value}) cannot be created. {ex.Message}");
                    throw new RepositoryException($"Base directory ({value}) cannot be created. {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// The directory in which the provider works.
        /// </summary>
        protected string WorkingDirectory
        {
            get => _workingDirectory;
            set
            {
                try
                {
                    Directory.CreateDirectory(value);
                    _workingDirectory = value;
                }
                catch (Exception)
                {
                    _logger.Error($"Working directory ({value}) does not exist.");
                    throw new RepositoryException($"Working directory ({value}) does not exist.");
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


            BaseDirectory = reader.GetValue("WorkingDirectoryBase", typeof(string)).ToString();
            if (string.IsNullOrWhiteSpace(BaseDirectory) || string.Equals(BaseDirectory.ToLower(), "desktop"))
            {
                BaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            WorkingDirectory = Path.Combine(BaseDirectory, reader.GetValue("WorkingDirectory", typeof(string)).ToString());
            BackupDirectory = Path.Combine(WorkingDirectory, reader.GetValue("BackupDirectory", typeof(string)).ToString());

            _separator = reader.GetValue("CsvSeparator", typeof(string)).ToString();
            _dateFormat = reader.GetValue("DateFormatForFileName", typeof(string)).ToString();
            _fileNameExtension = reader.GetValue("CsvFileNameExtension", typeof(string)).ToString();
            _cultureInfo = new CultureInfo(reader.GetValue("CultureInfo", typeof(string)).ToString());

            _logger.Debug($"Base directory is {BaseDirectory} from config file.");
            _logger.Debug($"Working directory is {WorkingDirectory} from config file.");
            _logger.Debug($"Backup directory is {BackupDirectory} from config file.");

            _logger.Debug($"File name for file writing is {_fileName} from config file.");
            _logger.Debug($"File name extension is {_fileNameExtension} from config file.");
            _logger.Debug($"Culture info for file writing is {_cultureInfo} from config file.");
            _logger.Debug($"CSV separator is {_separator} from config file.");
    }

    internal void SaveChanges(string[] header, IEnumerable<string> content, string fullPath, string separator)
        {
            _logger.Info($"Saving changes into {Path.GetFileName(fullPath)} ...");

            try
            {
                List<string> contentWithHeader = AddHeader(header, separator);
                contentWithHeader.AddRange(content);
                var stringContent = string.Join("\n", contentWithHeader);

                _fileSystemFacade.Save(fullPath, stringContent);
                _logger.Info($"{Path.GetFileName(fullPath)} saved.");
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
            _logger.Info("Creating backup ...");

            try
            {
                _fileSystemFacade.Backup(
                    Path.Combine(workingDir, fileName),
                    Path.Combine(
                        backupDir,
                        $"{Path.GetFileNameWithoutExtension(fileName)} {DateTime.Now.ToString(_dateFormat)}{Path.GetExtension(fileName)}"));
                _logger.Info($"{fileName} backed up.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error when saving changes. {ex.Message}");
                throw new RepositoryException($"Error when creating backup.", ex);
            }
        }
    }
}
