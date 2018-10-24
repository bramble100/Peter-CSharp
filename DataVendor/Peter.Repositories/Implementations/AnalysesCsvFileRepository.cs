using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using Peter.Models.Interfaces;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;

namespace Peter.Repositories.Implementations
{
    public class AnalysesCsvFileRepository : CsvFileRepository, IAnalysesRepository
    {
        private Dictionary<string, IAnalysis> _entities;

        public AnalysesCsvFileRepository() : base()
        {
            var reader = new AppSettingsReader();

            _workingDirectory = Path.Combine(
                _workingDirectory,
                reader.GetValue("WorkingDirectoryAnalyses", typeof(string)).ToString());

            _entities = new Dictionary<string, IAnalysis>();
        }

        public void Add(KeyValuePair<string, IAnalysis> analysis) =>
            _entities.Add(analysis.Key, analysis.Value);

        public void AddRange(IEnumerable<KeyValuePair<string, IAnalysis>> analyses)
        {
            analyses.ToList().ForEach(Add);
            _logger.Info($"{analyses.Count()} new analyses added.");
        }

        public IFinancialAnalysis Find(string isin) => throw new System.NotImplementedException();

        public Dictionary<string, IAnalysis> GetAll() => _entities;

        public void SaveChanges()
        {
            var reader = new AppSettingsReader();
            _fileName = $"{reader.GetValue("AnalysesFileName", typeof(string)).ToString()}" +
                $" {DateTime.Now.ToString(reader.GetValue("DateFormatForFileName", typeof(string)).ToString())}" +
                $".{reader.GetValue("CsvFileNameExtension", typeof(string)).ToString()}";

            SaveChanges(
                CsvLineAnalysis.Header,
                _entities.Select(e => CsvLineAnalysis.FormatForCSV(e, _separator, new CultureInfo("hu-HU"))),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }
    }
}
