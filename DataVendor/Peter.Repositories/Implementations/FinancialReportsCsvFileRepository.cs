﻿using System;
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
    public class FinancialReportsCsvFileRepository : CsvFileRepository, IFinancialReportsCsvFileRepository
    {
        private readonly Dictionary<string, IFinancialAnalysis> _analyses;

        public FinancialReportsCsvFileRepository() : base()
        {
            var reader = new AppSettingsReader();

            _workingDirectory = Path.Combine(
                _workingDirectory,
                reader.GetValue("WorkingDirectoryAnalyses", typeof(string)).ToString());

            _analyses = new Dictionary<string, IFinancialAnalysis>();
            Load();
        }

        private void Load()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, IFinancialAnalysis> Entities => throw new System.NotImplementedException();

        public void Add(IFinancialAnalysis analysis)
        {
            throw new System.NotImplementedException();
        }

        public IFinancialAnalysis Find(string isin)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string isin)
        {
            throw new System.NotImplementedException();
        }

        public void SaveChanges()
        {
            var reader = new AppSettingsReader();
            _fileName = $"{reader.GetValue("AnalysesFileName", typeof(string)).ToString()}" +
                $" {DateTime.Now.ToString(reader.GetValue("DateFormatForFileName", typeof(string)).ToString())}" +
                $".{reader.GetValue("CsvFileNameExtension", typeof(string)).ToString()}";

            _header = new string[]
            {
                "Name",
                "ISIN",
            };

            base.SaveChanges(
                _header,
                Entities.Select(e => CsvLineFinancialAnalysis.FormatForCSV(e, ";", new CultureInfo("hu-HU"))),
                Path.Combine(_workingDirectory, _fileName));
        }
    }
}
