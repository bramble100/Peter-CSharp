using System.Collections.Generic;

namespace Infrastructure
{
    public class Config : IConfig
    {
        public T GetValue<T>(string key)
        {
            Dictionary<string, object> _configuration = new Dictionary<string, object>()
            {
                { "BackupDirectory", "Backups" },
                { "CsvFileNameExtension", "csv"},
                { "CsvSeparator", ";"},
                { "CultureInfo", "hu-HU"},
                { "DateFormatForFileName", "yyyy-MM-dd-HH-mm"},
                { "FetchNewMarketData", "fetch" },
                { "IsinFileName", "isins.csv" },
                { "MarketDataFileName", "marketdata.csv" },
                { "RegistryFileName", "registry.csv" },
                { "UpdateMarketDataWithISINs", "addisins" },
                { "WorkingDirectory", "StockExchange" },
                { "WorkingDirectoryBase", "Desktop" },
                { "WorkingDirectoryRawDownloads", "RawDownload" }
            };

            return (T)_configuration[key];
        }
    }
}
