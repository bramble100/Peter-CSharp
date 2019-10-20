using Infrastructure;
using NLog;
using Peter.Repositories.Exceptions;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System;
using Models.Interfaces;

namespace Peter.Repositories.Implementations
{
    public class AnalysesCsvFileRepository : CsvFileRepository, IAnalysesRepository
    {
        private new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private Dictionary<string, IAnalysis> _entities;

        public AnalysesCsvFileRepository(
            IConfigReader config,
            IFileSystemFacade fileSystemFacade)
            : base(config, fileSystemFacade)
        {
            WorkingDirectory = Path.Combine(
                WorkingDirectory,
                _configReader.Settings.WorkingDirectoryAnalyses);
            _logger.Debug($"Working directory is {WorkingDirectory} from config file.");

            _entities = new Dictionary<string, IAnalysis>();
        }

        public void Add(KeyValuePair<string, IAnalysis> analysis)
        {
            _entities.Add(analysis.Key, analysis.Value);
            _fileContentSaved = false;
        }

        public void AddRange(IEnumerable<KeyValuePair<string, IAnalysis>> analyses)
        {
            if (analyses == null)
            {
                throw new ArgumentNullException(nameof(analyses));
            }

            if (!analyses.Any())
            {
                _logger.Debug("No analysis to add.");
                return;
            }

            try
            {
                foreach (var analysis in analyses)
                {
                    Add(analysis);
                }
                _fileContentSaved = false;
                _logger.Info($"{analyses.Count()} new analyses added.");
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
                _logger.Debug(ex);
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public IDictionary<string, IAnalysis> GetAll() => _entities.ToImmutableDictionary();

        public void SaveChanges()
        {
            if (_fileContentSaved)
            {
                _logger.Debug("Content is already saved into file.");
                return;
            }

            _fileName = _configReader.Settings.AnalysesFileName +
                $" {DateTime.Now.ToString(_configReader.Settings.DateFormatForFileName)}." +
                _configReader.Settings.CsvFileNameExtension;

            SaveChanges(
                CsvLineAnalysis.Header,
                _entities.Select(e => CsvLineAnalysis.FormatForCSV(e, _separator, new CultureInfo("hu-HU"))),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }
    }
}
