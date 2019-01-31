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
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class IsinsCsvFileRepository : CsvFileRepository, IIsinsRepository
    {
        protected new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private Dictionary<string, string> _isins;

        /// <summary>
        /// Constructor.
        /// </summary>
        public IsinsCsvFileRepository() : base()
        {
            _fileName = new AppSettingsReader().GetValue("IsinFileName", typeof(string)).ToString();
            _logger.Debug($"Isin filename is {_fileName} from config file.");

            _isins = new Dictionary<string, string>();
            Load();
        }

        public void Add(string name) => _isins.Add(name, string.Empty);

        public bool ContainsName(string name) => _isins.ContainsKey(name);

        public string GetIsinByCompanyName(string name) => _isins[name];

        public ImmutableHashSet<string> GetNames() => _isins.Keys.ToImmutableHashSet();

        public void Remove(string name) => _isins.Remove(name);

        public void SaveChanges()
        {
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
                    File.ReadLines(fullPath, Encoding.UTF8).FirstOrDefault());

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
