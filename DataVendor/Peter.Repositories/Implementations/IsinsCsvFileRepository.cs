using Microsoft.VisualBasic.FileIO;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Exceptions;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class IsinsCsvFileRepository : CsvFileRepository, IIsinsCsvFileRepository
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public IsinsCsvFileRepository() : base()
        {
            _fileName = new AppSettingsReader().GetValue("IsinFileName", typeof(string)).ToString();
        }

        /// <summary>
        /// Loads the CSV file and stores its content.
        /// </summary>
        /// <returns></returns>
        public INameToIsin Load()
        {
            try
            {
                var filePath = Path.Combine(_workingDirectory, _fileName);
                INameToIsin isins = new NameToIsin();

                using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
                {
                    parser.SetDelimiters(_separator);

                    RemoveHeader(parser);

                    while (!parser.EndOfData)
                    {
                        isins.Add(parser.ReadFields());
                    }
                }

                return isins;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error when loading entities in {GetType().Name}.");
                throw new RepositoryException($"Error when loading entities in {GetType().Name}.", ex);
            }
        }

        public void Save(INameToIsin isins)
        {
            CreateBackUp(
                WorkingDirectory,
                BackupDirectory,
                _fileName);
            SaveChanges(
                CsvLineIsin.Header,
                // TODO use CsvLineMarketData for CSV formatting
                isins.Select(i => i.FormatterForCSV(_separator)),
                Path.Combine(WorkingDirectory, _fileName),
                _separator);
        }
    }
}
