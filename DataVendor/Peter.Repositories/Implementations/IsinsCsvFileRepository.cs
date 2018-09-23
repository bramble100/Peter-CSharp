using Microsoft.VisualBasic.FileIO;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System.Collections.Generic;
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

            _header = new string[] { "Name", "ISIN" };
        }

        /// <summary>
        /// Loads the CSV file and stores its content.
        /// </summary>
        /// <returns></returns>
        public INameToIsin Load()
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

        public void Save(INameToIsin isins)
        {
            List<string> strings = AddHeader();

            strings.AddRange(isins.Select(i => i.FormatterForCSV(_separator)));

            File.WriteAllLines(
                Path.Combine(WorkingDirectory, _fileName),
                strings,
                Encoding.UTF8);
        }
    }
}
