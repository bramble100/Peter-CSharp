using Newtonsoft.Json;
using NLog;
using System;

namespace Infrastructure
{
    public class ConfigReader : IConfigReader
    {
        private readonly Logger _logger;

        public ConfigSettings Settings { get; private set; }

        public ConfigReader(IFileSystemFacade facade)
        {
            _logger = LogManager.GetCurrentClassLogger();

            try
            {
                using (var stream = facade.Open("config.json"))
                {
                    Settings = JsonConvert.DeserializeObject<ConfigSettings>(stream.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public class ConfigSettings
        {
            public string AnalysesFileName { get; set; }
            public string BackupDirectory { get; set; }
            public int BuyingPacketInEuro { get; set; }
            public string CsvFileNameExtension { get; set; }
            public string CsvSeparator { get; set; }
            public string CultureInfo { get; set; }
            public string DateFormatForFileName { get; set; }
            public int FastMovingAverage { get; set; }
            public string FetchNewMarketData { get; set; }
            public string IsinFileName { get; set; }
            public string MarketDataFileName { get; set; }
            public string RegistryFileName { get; set; }
            public int SlowMovingAverage { get; set; }
            public string UpdateMarketDataWithISINs { get; set; }
            public string WorkingDirectory { get; set; }
            public string WorkingDirectoryAnalyses { get; set; }
            public string WorkingDirectoryBase { get; set; }
            public string WorkingDirectoryRawDownloads { get; set; }
            public string WorkingDirectoryRegistry { get; set; }
        }
    }
}
