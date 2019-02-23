﻿using Infrastructure;
using Microsoft.VisualBasic.FileIO;
using NLog;
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
    public class MarketDataCsvFileRepository : CsvFileRepository, IMarketDataRepository
    {
        protected new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly HashSet<IMarketDataEntity> _entities;

        public MarketDataCsvFileRepository(
            IConfigReader config,
            IFileSystemFacade fileSystemFacade)
            : base(config, fileSystemFacade)
        {
            WorkingDirectory = Path.Combine(
                WorkingDirectory,
                _configReader.Settings.WorkingDirectoryRawDownloads);
            _logger.Debug($"Working directory is {WorkingDirectory} from config file.");

            _fileName = _configReader.Settings.MarketDataFileName;
            _logger.Debug($"Market data filename is {_fileName} from config file.");

            _entities = new HashSet<IMarketDataEntity>();
        }

        public IEnumerable<string> Isins
        {
            get
            {
                if (!_fileContentLoaded) Load();

                return _entities.Select(e => e.Isin).Distinct().ToImmutableList();
            }
        }

        public void AddRange(IEnumerable<IMarketDataEntity> entities)
        {
            if (entities is null)
                throw new ArgumentNullException(nameof(entities));

            if (!entities.Any()) return;

            var entitiesSet = new HashSet<IMarketDataEntity>(entities);

            if (!_fileContentLoaded) Load();

            _logger.Info("Adding market data entities ...");
            entitiesSet.ExceptWith(_entities);
            _entities.UnionWith(entitiesSet);
            _logger.Info($"{entitiesSet.Count} new market data entities added.");

            _fileContentSaved = !entitiesSet.Any();
        }

        public IEnumerable<IMarketDataEntity> GetAll()
        {
            if (!_fileContentLoaded) Load();

            return _entities.ToImmutableList();
        }

        public void SaveChanges()
        {
            if (!_fileContentLoaded)
            {
                _logger.Debug("No need to save the data, nothing was loaded.");
                return;
            }

            if (_fileContentSaved)
            {
                _logger.Debug("No need to save the data, nothing changed since last save.");
                return;
            }

            CreateBackUp(
                WorkingDirectory,
                BackupDirectory,
                _fileName);

            SaveChanges(
                CsvLineMarketData.Header,
                _entities.Select(e => CsvLineMarketData.FormatForCSV(e, _separator, _cultureInfo)),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }

        private void Load()
        {
            try
            {
                var fullPath = Path.Combine(WorkingDirectory, _fileName);

                using(var reader = _fileSystemFacade.Open(fullPath))
                {
                    var baseInfo = GetCsvSeparatorAndCultureInfo(reader.ReadLine());
                    _separator = baseInfo.Item1;
                    _cultureInfo = baseInfo.Item2;

                    _logger.Debug($"{_fileName}: separator: \"{_separator}\" culture: \"{_cultureInfo}\".");
                    _logger.Info("Loading market data entities from CSV file ...");

                    using (var parser = new TextFieldParser(reader))
                    {
                        parser.SetDelimiters(_separator);

                        while (!parser.EndOfData)
                        {
                            if (CsvLineMarketData.TryParseFromCsv(
                                parser.ReadFields(),
                                _cultureInfo,
                                out IMarketDataEntity result))
                            {
                                _entities.Add(result);
                            }
                        }
                    }
                }

                _fileContentLoaded = true;
                _fileContentSaved = true;

                _logger.Info($"{_entities.Count} new market data entities loaded.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error when loading entities in {GetType().Name}.");
                throw new RepositoryException($"Error when loading entities in {GetType().Name}.", ex);
            }
        }
    }
}