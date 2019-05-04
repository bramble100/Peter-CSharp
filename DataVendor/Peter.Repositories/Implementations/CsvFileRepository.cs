using Infrastructure;
using NLog;
using Peter.Repositories.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Peter.Repositories.Implementations
{
    public abstract class CsvFileRepository
    {
        protected readonly Logger _logger;

        protected readonly string _fileNameExtension;
        protected readonly IConfigReader _configReader;
        protected readonly IFileSystemFacade _fileSystemFacade;
        
        protected CultureInfo _cultureInfo;
        protected string _fileName;
        protected bool _fileContentLoaded;
        protected bool _fileContentSaved;
        protected string _separator;

        private readonly string _dateFormat;

        /// <summary>
        /// The directory in which the provider saves backups.
        /// </summary>
        protected string BackupDirectory { get; set; }

        /// <summary>
        /// The root directory in which the provider saves everything.
        /// </summary>
        protected string BaseDirectory { get; set; }

        /// <summary>
        /// The directory in which the provider works.
        /// </summary>
        protected string WorkingDirectory { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CsvFileRepository(
            IConfigReader config,
            IFileSystemFacade fileSystemFacade)
        {
            _configReader = config;
            _fileSystemFacade = fileSystemFacade;
            _logger = LogManager.GetCurrentClassLogger();

            BaseDirectory = _configReader.Settings.WorkingDirectoryBase;
            if (string.IsNullOrWhiteSpace(BaseDirectory) || string.Equals(BaseDirectory.ToLower(), "desktop"))
            {
                BaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            WorkingDirectory = Path.Combine(BaseDirectory, _configReader.Settings.WorkingDirectory);
            BackupDirectory = Path.Combine(WorkingDirectory, _configReader.Settings.BackupDirectory);

            _separator = _configReader.Settings.CsvSeparator;
            _dateFormat = _configReader.Settings.DateFormatForFileName;
            _fileNameExtension = _configReader.Settings.CsvFileNameExtension;
            _cultureInfo = new CultureInfo(_configReader.Settings.CultureInfo);

            _logger.Debug($"Base directory is {BaseDirectory} from config file.");
            _logger.Debug($"Working directory is {WorkingDirectory} from config file.");
            _logger.Debug($"Backup directory is {BackupDirectory} from config file.");

            _logger.Debug($"File name for file writing is {_fileName} from config file.");
            _logger.Debug($"File name extension is {_fileNameExtension} from config file.");
            _logger.Debug($"Culture info for file writing is {_cultureInfo} from config file.");
            _logger.Debug($"CSV separator is {_separator} from config file.");
        }

        internal void SaveChanges(
            string[] header,
            IEnumerable<string> content,
            string fullPath,
            string separator)
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

        protected Tuple<string, CultureInfo> GetCsvSeparatorAndCultureInfo(string header)
        {
            if (header is null)
                throw new ArgumentNullException(nameof(header));
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException(nameof(header));

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
