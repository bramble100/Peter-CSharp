﻿using Microsoft.VisualBasic.FileIO;
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
        private readonly IRegistry _entities;

        public IEnumerable<string> Isins => _entities.Select(e => e.Key);

        public RegistryCsvFileRepository() : base()
        {
            var reader = new AppSettingsReader();

            _workingDirectory = Path.Combine(
                _workingDirectory,
                reader.GetValue("WorkingDirectoryRegistry", typeof(string)).ToString());
            _fileName = reader.GetValue("RegistryFileName", typeof(string)).ToString();

            _entities = new Registry();
            Load();
        }

        private void Load()
        {
            try
            {
                Tuple<string, CultureInfo> baseInfo;
                var filePath = Path.Combine(_workingDirectory, _fileName);

                baseInfo = GetCsvSeparatorAndCultureInfo(
                    File.ReadLines(filePath, Encoding.UTF8).FirstOrDefault());

                _logger.Info($"{_fileName}: separator: \"{baseInfo.Item1}\" culture: \"{baseInfo.Item2.ToString()}\".");

                using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
                {
                    parser.SetDelimiters(baseInfo.Item1);
                    RemoveHeader(parser);
                    while (!parser.EndOfData)
                    {
                        if (CsvLineRegistryEntryWithIsin.TryParseFromCsv(
                            parser.ReadFields(),
                            baseInfo.Item2,
                            out KeyValuePair<string, IRegistryEntry> result))
                            _entities.Add(result);
                    }
                }

                _logger.Info($"{_entities.Count} new registry item loaded.");
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
                _entities.Select(e => CsvLineRegistryEntryWithIsin.FormatForCSV(e, _separator, new CultureInfo("hu-HU"))),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }

        public void AddRange(IEnumerable<KeyValuePair<string, IRegistryEntry>> newEntries)
        {
            newEntries.ToList().ForEach(e => _entities.Add(e));
            _logger.Info($"{newEntries.Count()} new registry item added.");
        }

        public void RemoveRange(IEnumerable<string> isins)
        {
            isins.ToList().ForEach(e => _entities.Remove(e));
            _logger.Info($"{isins.Count()} registry item removed.");
        }

        public IRegistry GetAll() => _entities.Select(e => e) as IRegistry;
    }
}