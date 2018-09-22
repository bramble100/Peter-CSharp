using Microsoft.VisualBasic.FileIO;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using Peter.Repositories.Helpers;
using Peter.Repositories.Interfaces;
using System.Configuration;
using System.IO;
using System.Text;

namespace Peter.Repositories.Implementations
{
    public class RegistryCsvFileRepository : CsvFileRepository, IRegistryCsvFileRepository
    {
        public RegistryCsvFileRepository() : base()
        {
            _fileName = new AppSettingsReader().GetValue("RegistryFileName", typeof(string)).ToString();

            _header = new string[] 
            {
                "Name",
                "ISIN",
                "Stock Exchange Link",
                "Own Investor Link",
                "EPS",
                "Months in Report",
                "Next Report Date",
                "Position"
            };
        }

        public IRegistry Load()
        {
            var filePath = Path.Combine(_workingDirectory, _fileName);
            IRegistry registry = new Registry();

            using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.SetDelimiters(_separator);

                RemoveHeader(parser);

                while (!parser.EndOfData)
                {
                    if(CsvLineRegistryEntryWithIsin.TryParseFromCsv(parser.ReadFields(), out var result))
                    registry.Add(result);
                }
            }

            return registry;
        }

        public void Save(IRegistry entities)
        {
            throw new System.NotImplementedException();
        }
    }
}