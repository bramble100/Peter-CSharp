using Infrastructure;
using Microsoft.VisualBasic.FileIO;
using NLog;
using Peter.Models.Interfaces;
using Peter.Repositories.Exceptions;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class RegistryCsvFileRepository : CsvFileRepository, IRegistryRepository
    {
        protected new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ICollection<IRegistryEntry> _entities;

        public IEnumerable<string> Isins => _entities.Select(e => e.Isin);

        public RegistryCsvFileRepository(
            IConfig config, 
            IFileSystemFacade fileSystemFacade) 
            : base(config, fileSystemFacade)
        {
            WorkingDirectory = Path.Combine(
                WorkingDirectory,
                _config.GetValue<string>("WorkingDirectoryRegistry"));
            _logger.Debug($"Working directory is {WorkingDirectory} from config file.");

            _fileName = _config.GetValue<string>("RegistryFileName");
            _logger.Debug($"Market data filename is {_fileName} from config file.");

            _entities = new List<IRegistryEntry>();
        }

        public void AddRange(IEnumerable<IRegistryEntry> entries)
        {
            if (entries is null)
                throw new ArgumentNullException(nameof(entries));

            if (!_fileContentLoaded) Load();

            foreach (var entry in entries)
            {
                _entities.Add(entry);
            }

            _fileContentSaved = false;
        }

        public IRegistryEntry GetById(string isin)
        {
            if (!_fileContentLoaded) Load();

            return _entities.Where(e => e.Isin.Equals(isin)).SingleOrDefault();
        }

        public void RemoveRange(IEnumerable<string> isins)
        {
            if (isins is null)
                throw new ArgumentNullException(nameof(isins));

            if (!_fileContentLoaded) Load();

            isins
                .ToList()
                .ForEach(isin => _entities.Remove(_entities.SingleOrDefault(e => Equals(e.Isin, isin))));

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
            try
            {
                Tuple<string, CultureInfo> baseInfo;
                var fullPath = Path.Combine(WorkingDirectory, _fileName);

                baseInfo = GetCsvSeparatorAndCultureInfo(
                    _fileSystemFacade.ReadLines(fullPath, Encoding.UTF8).FirstOrDefault());

                _logger.Debug($"{_fileName}: separator: \"{baseInfo.Item1}\" culture: \"{baseInfo.Item2.ToString()}\".");

                using (var parser = new TextFieldParser(fullPath, Encoding.UTF8))
                {
                    parser.SetDelimiters(baseInfo.Item1);
                    RemoveHeader(parser);
                    while (!parser.EndOfData)
                    {
                        if (CsvLineRegistryEntryWithIsin.TryParseFromCsv(
                            parser.ReadFields(),
                            baseInfo.Item2,
                            out IRegistryEntry result))
                            _entities.Add(result);
                    }
                }

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
    }
}