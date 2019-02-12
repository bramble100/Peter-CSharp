using Infrastructure;
using Microsoft.VisualBasic.FileIO;
using NLog;
using Peter.Repositories.Exceptions;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Peter.Repositories.Implementations
{
    public class IsinsCsvFileRepository : CsvFileRepository, IIsinsRepository
    {
        private new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        // TODO change to separate class (so that it can be persisted)
        private readonly Dictionary<string, string> _isins;

        /// <summary>
        /// Constructor for DI.
        /// </summary>
        /// <param name="fileSystemFacade"></param>
        public IsinsCsvFileRepository(
            IConfig config,
            IFileSystemFacade fileSystemFacade) 
            : base(config, fileSystemFacade)
        {
            _fileName = _config.GetValue<string>("IsinFileName");
            _logger.Debug($"Isin filename is {_fileName} from config file.");

            _isins = new Dictionary<string, string>();
        }

        public void Add(string name)
        {
            if (!_fileContentLoaded) Load();

            _isins.Add(name, string.Empty);
            _fileContentSaved = false;
        }

        public bool ContainsName(string name)
        {
            if (!_fileContentLoaded) Load();

            return _isins.ContainsKey(name);
        }

        public string GetIsinByCompanyName(string name)
        {
            if (!_fileContentLoaded) Load();

            return _isins[name];
        }

        public ImmutableHashSet<string> GetNames()
        {
            if (!_fileContentLoaded) Load();

            return _isins.Keys.ToImmutableHashSet();
        }

        public void Remove(string name)
        {
            if (!_fileContentLoaded) Load();

            _isins.Remove(name);
            _fileContentSaved = false;
        }

        public void SaveChanges()
        {
            if (!_fileContentLoaded || _fileContentSaved) return;

            CreateBackUp(
                WorkingDirectory,
                BackupDirectory,
                _fileName);
            SaveChanges(
                CsvLineIsin.Header,
                // TODO use CsvLineIsin for CSV formatting
                _isins.Select(i => i.FormatterForCSV(_separator)),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }

        private void Load()
        {
            try
            {
                var fullPath = Path.Combine(WorkingDirectory, _fileName);

                using (var reader = _fileSystemFacade.Open(fullPath))
                {
                    var baseInfo = GetCsvSeparatorAndCultureInfo(reader.ReadLine());
                    _separator = baseInfo.Item1;
                    _cultureInfo = baseInfo.Item2;

                    _logger.Debug($"{_fileName}: separator: \"{_separator}\" culture: \"{_cultureInfo}\".");
                    _logger.Info("Loading ISINs from CSV file ...");

                    using (var parser = new TextFieldParser(reader))
                    {
                        parser.SetDelimiters(_separator);

                        while (!parser.EndOfData)
                        {
                            if (CsvLineIsin.TryParseFromCsv(parser.ReadFields(), out var result))
                            {
                                _isins.Add(result.Key, result.Value);
                            }
                        }
                    }
                }

                _fileContentLoaded = true;
                _fileContentSaved = true;

                _logger.Info($"{_isins.Count} new ISIN entries loaded.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error when loading entities in {GetType().Name}.");
                throw new RepositoryException($"Error when loading entities in {GetType().Name}.", ex);
            }
        }
    }
}
