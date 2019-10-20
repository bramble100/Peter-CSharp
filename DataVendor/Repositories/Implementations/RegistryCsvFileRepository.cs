using Infrastructure;
using Microsoft.VisualBasic.FileIO;
using Models.Interfaces;
using NLog;
using Repositories.Exceptions;
using Repositories.Helpers;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Repositories.Implementations
{
    public class RegistryCsvFileRepository : CsvFileRepository, IRegistryRepository
    {
        protected new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly HashSet<IRegistryEntry> _entities;

        public IEnumerable<string> Isins
        {
            get
            {
                if (!_fileContentLoaded) Load();

                return _entities.Select(e => e.Isin).Distinct().ToImmutableArray();
            }
        }

        public RegistryCsvFileRepository(
            IConfigReader config, 
            IFileSystemFacade fileSystemFacade) 
            : base(config, fileSystemFacade)
        {
            string fullPath;

            WorkingDirectory = Path.Combine(
                WorkingDirectory,
                _configReader.Settings.WorkingDirectoryRegistry);
            _logger.Debug($"Working directory is {WorkingDirectory} from config file.");

            _fileName = _configReader.Settings.RegistryFileName;
            _logger.Debug($"Market data filename is {_fileName} from config file.");

            try
            {
                fullPath = Path.Combine(WorkingDirectory, _fileName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw new RepositoryException(ex.Message, ex);
            }

            if (!File.Exists(fullPath))
            {
                var message = new FileNotFoundException().Message;
                var ex = new FileNotFoundException(message, fullPath);
                _logger.Error(ex, message);
                throw new RepositoryException(
                    message,
                    new FileNotFoundException(message, fullPath));
            }
                

            _entities = new HashSet<IRegistryEntry>();
        }

        public void AddRange(IEnumerable<IRegistryEntry> entries)
        {
            if (entries is null)
                throw new ArgumentNullException(nameof(entries));

            if (!entries.Any()) return;

            if (!_fileContentLoaded) Load();

            entries
                .ToList()
                .ForEach(e => _entities.Add(e));

            _fileContentSaved = false;
        }

        public IRegistryEntry GetById(string isin)
        {
            if (!_fileContentLoaded) Load();

            var entity = _entities.FirstOrDefault(e => string.Equals(e.Isin, isin));
            _logger.Debug($"Searching for {isin}, found: {(entity is null ? "null" : entity.ToString())}");
            return entity;
        }

        public void RemoveRange(IEnumerable<string> isins)
        {
            if (isins is null)
            {
                throw new ArgumentNullException(nameof(isins));
            }

            if (!isins.Any())
            {
                _logger.Info("No entry to remove.");
                return;
            }

            if (!_fileContentLoaded)
            {
                Load();
            } 

            isins
                .ToList()
                .ForEach(isin => _entities.Remove(_entities.Single(e => Equals(e.Isin, isin))));

            _fileContentSaved = false;

            _logger.Info($"{isins.Count()} registry item removed.");
        }

        public void SaveChanges()
        {
            if (!_fileContentLoaded || _fileContentSaved) return;

            CreateBackUp(
                WorkingDirectory, 
                BackupDirectory, 
                _fileName);
            SaveChanges(
                CsvLineRegistryEntryWithIsin.Header,
                _entities.Select(e => CsvLineRegistryEntryWithIsin.FormatForCSV(e, _separator, new CultureInfo("hu-HU"))),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }

        private void Load()
        {
            _logger.Info("Loading registry entries ...");

            try
            {
                var fullPath = Path.Combine(WorkingDirectory, _fileName);

                LoadWithReader(fullPath);

                _fileContentLoaded = true;
                _fileContentSaved = true;

                _logger.Info($"{_entities.Count} new registry item loaded.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error when loading entities in {GetType().Name}.");
                throw new RepositoryException($"Error when loading entities in {GetType().Name}.", ex);
            }
        }

        private void LoadWithReader(string fullPath)
        {
            using (var reader = _fileSystemFacade.Open(fullPath))
            {
                var baseInfo = GetCsvSeparatorAndCultureInfo(reader.ReadLine());
                _separator = baseInfo.Item1;
                _cultureInfo = baseInfo.Item2;
                _logger.Debug($"{_fileName}: separator: \"{_separator}\" culture: \"{_cultureInfo}\".");
                _logger.Info("Loading registry entries from CSV file ...");

                LoadWithParser(reader);
            }
        }

        private void LoadWithParser(StreamReader reader)
        {
            using (var parser = new TextFieldParser(reader))
            {
                parser.SetDelimiters(_separator);

                while (!parser.EndOfData)
                {
                    if (CsvLineRegistryEntryWithIsin.TryParseFromCsv(
                        parser.ReadFields(),
                        _cultureInfo,
                        out IRegistryEntry result) && result != null)
                        _entities.Add(result);
                }
            }
        }
    }
}