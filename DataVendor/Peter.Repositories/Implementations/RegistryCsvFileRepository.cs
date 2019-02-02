using Microsoft.VisualBasic.FileIO;
using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Exceptions;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class RegistryCsvFileRepository : CsvFileRepository, IRegistryRepository
    {
        protected new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IRegistry _entitiesOld;
        private readonly List<IRegistryEntry> _entities;

        public IEnumerable<string> GetIsins => _entitiesOld.Select(e => e.Key);

        public IEnumerable<string> Isins => _entitiesOld.Keys;

        public RegistryCsvFileRepository() : base()
        {
            var reader = new AppSettingsReader();

            WorkingDirectory = Path.Combine(
                WorkingDirectory,
                reader.GetValue("WorkingDirectoryRegistry", typeof(string)).ToString());
            _logger.Debug($"Working directory is {WorkingDirectory} from config file.");

            _fileName = reader.GetValue("RegistryFileName", typeof(string)).ToString();
            _logger.Debug($"Market data filename is {_fileName} from config file.");

            _entitiesOld = new Registry();
            Load();
        }

        public void AddRange(IEnumerable<IRegistryEntry> entries) => 
            _entities.AddRange(entries ?? throw new ArgumentNullException(nameof(entries)));

        public IRegistry GetAll() => new Registry(_entitiesOld.Select(e => e));

        public IRegistryEntry GetById(string isin) => _entities.Where(e => e.Isin.Equals(isin)).SingleOrDefault();

        private void Load()
        {
            try
            {
                Tuple<string, CultureInfo> baseInfo;
                var fullPath = Path.Combine(WorkingDirectory, _fileName);

                baseInfo = GetCsvSeparatorAndCultureInfo(
                    File.ReadLines(fullPath, Encoding.UTF8).FirstOrDefault());

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
                            out KeyValuePair<string, IRegistryEntry> result))
                            _entitiesOld.Add(result);
                    }
                }

                _logger.Info($"{_entitiesOld.Count} new registry item loaded.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error when loading entities in {GetType().Name}.");
                throw new RepositoryException($"Error when loading entities in {GetType().Name}.", ex);
            }
        }

        public void SaveChanges()
        {
            CreateBackUp(
                WorkingDirectory, 
                BackupDirectory, 
                _fileName);
            SaveChanges(
                CsvLineRegistryEntryWithIsin.Header,
                _entitiesOld.Select(e => CsvLineRegistryEntryWithIsin.FormatForCSV(e, _separator, new CultureInfo("hu-HU"))),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }

        public void AddRange(IEnumerable<KeyValuePair<string, IRegistryEntry>> newEntries)
        {
            newEntries.ToList().ForEach(e => _entitiesOld.Add(e));
            _logger.Info($"{newEntries.Count()} new registry item added.");
        }

        public void RemoveRange(IEnumerable<string> isins)
        {
            isins.ToList().ForEach(e => _entitiesOld.Remove(e));
            _logger.Info($"{isins.Count()} registry item removed.");
        }
    }
}