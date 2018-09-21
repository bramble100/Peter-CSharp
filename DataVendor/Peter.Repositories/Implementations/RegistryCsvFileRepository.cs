using Peter.Models.Interfaces;
using Peter.Repositories.Interfaces;
using System.Configuration;

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
            throw new System.NotImplementedException();
        }

        public void Save(IRegistry entities)
        {
            throw new System.NotImplementedException();
        }
    }
}