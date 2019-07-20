using Infrastructure;
using Microsoft.VisualBasic.FileIO;
using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
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

        private readonly HashSet<INameToIsin> _entities;

        /// <summary>
        /// Constructor for DI.
        /// </summary>
        /// <param name="fileSystemFacade"></param>
        public IsinsCsvFileRepository(
            IConfigReader config,
            IFileSystemFacade fileSystemFacade)
            : base(config, fileSystemFacade)
        {
            _fileName = _configReader.Settings.IsinFileName;
            _logger.Debug($"Isin filename is {_fileName} from config file.");

            _entities = new HashSet<INameToIsin>();
        }

        public void Add(string name)
        {
            if (!_fileContentLoaded) Load();

            _entities.Add(new NameToIsin(name));

            _fileContentSaved = false;
        }

        public bool ContainsName(string name)
        {
            if (!_fileContentLoaded) Load();

            return _entities.Any(entity => string.Equals(entity.Name, name));
        }

        public string GetIsinByCompanyName(string name)
        {
            if (!_fileContentLoaded) Load();

            return _entities.SingleOrDefault(entity => string.Equals(entity.Name, name))?.Isin;
        }

        public IEnumerable<string> GetNames()
        {
            if (!_fileContentLoaded) Load();

            return _entities.Select(entity => entity.Name).ToImmutableArray();
        }

        public void Remove(string name)
        {
            if (!_fileContentLoaded) Load();

            _entities.RemoveWhere(entity => string.Equals(entity.Name, name));
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
                _entities.Select(i => CsvLineIsin.FormatForCSV(i, _separator)),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }

        private void Load()
        {
            _logger.Info("Loading new ISIN entries ...");

            try
            {
                var fullPath = Path.Combine(WorkingDirectory, _fileName);

                LoadWithReader(fullPath);

                _fileContentLoaded = true;
                _fileContentSaved = true;

                _logger.Info($"{_entities.Count} new ISIN entries loaded.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error when loading entities in IsinsCsvFileRepository.");
                throw new RepositoryException("Error when loading entities in IsinsCsvFileRepository.", ex);
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
                _logger.Info("Loading ISINs from CSV file ...");

                LoadWithParser(reader);
            }
        }

        private void LoadWithParser(StreamReader reader)
        {
            string[] fields;
            using (var parser = new TextFieldParser(reader))
            {
                parser.SetDelimiters(_separator);

                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();

                    if (CsvLineIsin.TryParseFromCsv(fields, out INameToIsin result))
                    {
                        _entities.Add(result);
                    }
                    else
                    {
                        _logger.Warn($"Fields (in a line) cannot be converted into NameToIsin", fields);
                    }
                }
            }
        }
    }
}
