using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Infrastructure;
using NLog;
using Peter.Models.Interfaces;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;

namespace Peter.Repositories.Implementations
{
    public class AnalysesCsvFileRepository : CsvFileRepository, IAnalysesRepository
    {
        protected new readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private Dictionary<string, IAnalysis> _entities;

        public AnalysesCsvFileRepository(
            IConfig config, 
            IFileSystemFacade fileSystemFacade) 
            : base(config, fileSystemFacade)
        {
            WorkingDirectory = Path.Combine(
                WorkingDirectory,
                _config.GetValue<string>("WorkingDirectoryAnalyses"));
            _logger.Debug($"Working directory is {WorkingDirectory} from config file.");

            _entities = new Dictionary<string, IAnalysis>();
        }

        public void Add(KeyValuePair<string, IAnalysis> analysis)
        {
            _entities.Add(analysis.Key, analysis.Value);
        }

        public void AddRange(IEnumerable<KeyValuePair<string, IAnalysis>> analyses)
        {
            if (analyses == null)
            {
                throw new ArgumentNullException(nameof(analyses));
            }

            analyses.ToList().ForEach(Add);
            _logger.Info($"{analyses.Count()} new analyses added.");
        }

        public IFinancialAnalysis Find(string isin) => throw new System.NotImplementedException();

        public IDictionary<string, IAnalysis> GetAll() => _entities;

        public void SaveChanges()
        {
            _fileName = $"{_config.GetValue<string>("AnalysesFileName")}" +
                $" {DateTime.Now.ToString(_config.GetValue<string>("DateFormatForFileName"))}" +
                $".{_config.GetValue<string>("CsvFileNameExtension")}";

            SaveChanges(
                CsvLineAnalysis.Header,
                _entities.Select(e => CsvLineAnalysis.FormatForCSV(e, _separator, new CultureInfo("hu-HU"))),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }
    }
}
