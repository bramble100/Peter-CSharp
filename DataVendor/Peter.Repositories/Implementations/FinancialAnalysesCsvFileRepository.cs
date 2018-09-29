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
    public class FinancialAnalysesCsvFileRepository : CsvFileRepository, IFinancialAnalysesCsvFileRepository
    {
        public FinancialAnalysesCsvFileRepository() : base()
        {
            var reader = new AppSettingsReader();

            _workingDirectory = Path.Combine(
                _workingDirectory,
                reader.GetValue("WorkingDirectoryAnalyses", typeof(string)).ToString());

            Entities = new Dictionary<string, IFinancialAnalysis>();
        }

        public Dictionary<string, IFinancialAnalysis> Entities { get; }

        public void Add(KeyValuePair<string, IFinancialAnalysis> analysis) =>
            Entities.Add(analysis.Key, analysis.Value);

        public void AddRange(IEnumerable<KeyValuePair<string, IFinancialAnalysis>> analyses) =>
            analyses.ToList().ForEach(Add);

        public IFinancialAnalysis Find(string isin) => throw new System.NotImplementedException();

        public void Remove(string isin) => throw new System.NotImplementedException();

        public void SaveChanges()
        {
            var reader = new AppSettingsReader();
            _fileName = $"{reader.GetValue("AnalysesFileName", typeof(string)).ToString()}" +
                $" {DateTime.Now.ToString(reader.GetValue("DateFormatForFileName", typeof(string)).ToString())}" +
                $".{reader.GetValue("CsvFileNameExtension", typeof(string)).ToString()}";

            SaveChanges(
                CsvLineFinancialAnalysis.Header,
                Entities.Select(e => CsvLineFinancialAnalysis.FormatForCSV(e, ";", new CultureInfo("hu-HU"))),
                Path.Combine(_workingDirectory, _fileName),
                ";");
        }
    }
}
