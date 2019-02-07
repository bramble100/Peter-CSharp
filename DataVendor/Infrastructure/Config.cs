using System.Collections.Generic;

namespace Infrastructure
{
    public class Config : IConfig
    {
        public T GetValue<T>(string key)
        {
            Dictionary<string, object> _configuration = new Dictionary<string, object>()
            {
                { "AnalysesFileName", "analyses" },
                { "BackupDirectory", "Backups" },
                { "BuyingPacketInEuro", 1000 },
                { "CsvFileNameExtension", "csv"},
                { "CsvSeparator", ";"},
                { "CultureInfo", "hu-HU"},
                { "DateFormatForFileName", "yyyy-MM-dd-HH-mm"},
                { "FastMovingAverage", 7},
                { "FetchNewMarketData", "fetch" },
                { "IsinFileName", "isins.csv" },
                { "MarketDataFileName", "marketdata.csv" },
                { "RegistryFileName", "registry.csv" },
                { "SlowMovingAverage", 21},
                { "UpdateMarketDataWithISINs", "addisins" },
                { "WorkingDirectory", "StockExchangeTest" },
                { "WorkingDirectoryAnalyses", "Analyses" },
                { "WorkingDirectoryBase", "Desktop" },
                { "WorkingDirectoryRawDownloads", "RawDownload" },
                { "WorkingDirectoryRegistry", "Registry" }
            };

            return (T)_configuration[key];
        }
    }
}
