using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;
using System.Linq;

namespace AnalysesManager.Services
{
    public class Service
    {
        private readonly IFinancialAnalysesCsvFileRepository _financialAnalysesCsvFileRepository;
        private readonly IMarketDataCsvFileRepository _marketDataCsvFileRepository;
        private readonly IRegistryCsvFileRepository _registryCsvFileRepository;

        public Service()
        {
            _financialAnalysesCsvFileRepository = new FinancialAnalysesCsvFileRepository();
            _marketDataCsvFileRepository = new MarketDataCsvFileRepository();
            _registryCsvFileRepository = new RegistryCsvFileRepository();
        }

        public void GenerateAnalyses()
        {
            var marketData = _marketDataCsvFileRepository.Load().ToList();
            var latestDate = marketData.Max(d => d.DateTime);
            var marketDataWithIsin = marketData
                .Where(d => !string.IsNullOrWhiteSpace(d.Isin));

            var latestDate = marketData;
            var isinsToAnalyse = marketData;
        }
    }
}
