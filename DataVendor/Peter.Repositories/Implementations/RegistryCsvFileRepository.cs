using Microsoft.VisualBasic.FileIO;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
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
    public class RegistryCsvFileRepository : CsvFileRepository, IRegistryCsvFileRepository
    {
        public IRegistry Entities { get; }

        public RegistryCsvFileRepository() : base()
        {
            var reader = new AppSettingsReader();

            _workingDirectory = Path.Combine(
                _workingDirectory,
                reader.GetValue("WorkingDirectoryRegistry", typeof(string)).ToString());
            _fileName = reader.GetValue("RegistryFileName", typeof(string)).ToString();

            Entities = new Registry();
            Load();
        }

        private void Load()
        {
            Tuple<string, CultureInfo> baseInfo;
            var filePath = Path.Combine(_workingDirectory, _fileName);

            try
            {
                baseInfo = GetCsvSeparatorAndCultureInfo(
                    File.ReadLines(filePath, Encoding.UTF8).FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }

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
                        Entities.Add(result);
                }
            }
        }

        public void SaveChanges()
        {
            List<string> strings = AddHeader(CsvLineRegistryEntryWithIsin.Header, ";");

            strings.AddRange(Entities.Select(e => CsvLineRegistryEntryWithIsin.FormatForCSV(e, ";", new CultureInfo("hu-HU"))));

            CreateBackUp(WorkingDirectory, BackupDirectory, _fileName);
            SaveActualFile(WorkingDirectory, _fileName, strings);
        }

        public void AddRange(IEnumerable<KeyValuePair<string, IRegistryEntry>> newEntries) => newEntries.ToList().ForEach(e => Entities.Add(e));

        public void RemoveRange(IEnumerable<string> isins) => isins.ToList().ForEach(e => Entities.Remove(e));
    }
}