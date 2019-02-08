using Infrastructure;
using Microsoft.VisualBasic.FileIO;
using NLog;
using Peter.Repositories.Exceptions;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class IsinsCsvFileRepository : CsvFileRepository, IIsinsRepository
    {
        private new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

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
        }

        public void SaveChanges()
        {
            if (!_fileContentLoaded) return;

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
                        if (CsvLineIsin.TryParseFromCsv(parser.ReadFields(), out var result))
                        {
                            _isins.Add(result.Key, result.Value);
                        }
                    }
                }
                _fileContentLoaded = true;
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
