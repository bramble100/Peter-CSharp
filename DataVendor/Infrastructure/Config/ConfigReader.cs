using Newtonsoft.Json;
using NLog;
using System;

namespace Infrastructure.Config
{
    /// <summary>
    /// Provides configuration settings.
    /// </summary>
    public class ConfigReader : IConfigReader
    {
        private readonly Logger _logger;

        /// <summary>
        /// Stores all the configuration settings.
        /// </summary>
        public IConfigSettings Settings { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="facade"></param>
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

        /// <summary>
        /// Class to store all the configuration settings.
        /// </summary>
        public interface IConfigSettings
        {
            /// <summary>
            /// File name for analyses.
            /// </summary>
            string AnalysesFileName { get; set; }
            /// <summary>
            /// Folder for backups.
            /// </summary>
            string BackupDirectory { get; set; }
            /// <summary>
            /// The sum for which shares to can be bought.
            /// </summary>
            int BuyingPacketInEuro { get; set; }
            /// <summary>
            /// Extension for CSV file name.
            /// </summary>
            string CsvFileNameExtension { get; set; }
            /// <summary>
            /// The separator character for saving CSV file.
            /// </summary>
            string CsvSeparator { get; set; }
            /// <summary>
            /// The culture info for saving CSV file.
            /// </summary>
            string CultureInfo { get; set; }
            /// <summary>
            /// DateFormat string for file name.
            /// </summary>
            string DateFormatForFileName { get; set; }
            /// <summary>
            /// The number of days for calculating fast moving average.
            /// </summary>
            int FastMovingAverage { get; set; }
            /// <summary>
            /// Command line parameter string for fetching new market data.
            /// </summary>
            string FetchNewMarketData { get; set; }
            /// <summary>
            /// File name for ISIN directory.
            /// </summary>
            string IsinFileName { get; set; }
            /// <summary>
            /// File name for market data.
            /// </summary>
            string MarketDataFileName { get; set; }
            /// <summary>
            /// File name for registry.
            /// </summary>
            string RegistryFileName { get; set; }
            /// <summary>
            /// The number of days for calculating slow moving average.
            /// </summary>
            int SlowMovingAverage { get; set; }
            /// <summary>
            /// Command line parameter string for adding ISINs to market data.
            /// </summary>
            string UpdateMarketDataWithISINs { get; set; }
            /// <summary>
            /// Working directory.
            /// </summary>
            string WorkingDirectory { get; set; }
            /// <summary>
            /// Folder for analyses.
            /// </summary>
            string WorkingDirectoryAnalyses { get; set; }
            /// <summary>
            /// Working directory base.
            /// </summary>
            string WorkingDirectoryBase { get; set; }
            /// <summary>
            /// Folder for raw market data.
            /// </summary>
            string WorkingDirectoryRawDownloads { get; set; }
            /// <summary>
            /// Folder for registry.
            /// </summary>
            string WorkingDirectoryRegistry { get; set; }
        }

        /// <summary>
        /// Class to store all the configuration settings.
        /// </summary>
        public class ConfigSettings : IConfigSettings
        {
            /// <summary>
            /// File name for analyses.
            /// </summary>
            public string AnalysesFileName { get; set; }
            /// <summary>
            /// Folder for backups.
            /// </summary>
            public string BackupDirectory { get; set; }
            /// <summary>
            /// The sum for which shares to can be bought.
            /// </summary>
            public int BuyingPacketInEuro { get; set; }
            /// <summary>
            /// Extension for CSV file name.
            /// </summary>
            public string CsvFileNameExtension { get; set; }
            /// <summary>
            /// The separator character for saving CSV file.
            /// </summary>
            public string CsvSeparator { get; set; }
            /// <summary>
            /// The culture info for saving CSV file.
            /// </summary>
            public string CultureInfo { get; set; }
            /// <summary>
            /// DateFormat string for file name.
            /// </summary>
            public string DateFormatForFileName { get; set; }
            /// <summary>
            /// The number of days for calculating fast moving average.
            /// </summary>
            public int FastMovingAverage { get; set; }
            /// <summary>
            /// Command line parameter string for fetching new market data.
            /// </summary>
            public string FetchNewMarketData { get; set; }
            /// <summary>
            /// File name for ISIN directory.
            /// </summary>
            public string IsinFileName { get; set; }
            /// <summary>
            /// File name for market data.
            /// </summary>
            public string MarketDataFileName { get; set; }
            /// <summary>
            /// File name for registry.
            /// </summary>
            public string RegistryFileName { get; set; }
            /// <summary>
            /// The number of days for calculating slow moving average.
            /// </summary>
            public int SlowMovingAverage { get; set; }
            /// <summary>
            /// Command line parameter string for adding ISINs to market data.
            /// </summary>
            public string UpdateMarketDataWithISINs { get; set; }
            /// <summary>
            /// Working directory.
            /// </summary>
            public string WorkingDirectory { get; set; }
            /// <summary>
            /// Folder for analyses.
            /// </summary>
            public string WorkingDirectoryAnalyses { get; set; }
            /// <summary>
            /// Working directory base.
            /// </summary>
            public string WorkingDirectoryBase { get; set; }
            /// <summary>
            /// Folder for raw market data.
            /// </summary>
            public string WorkingDirectoryRawDownloads { get; set; }
            /// <summary>
            /// Folder for registry.
            /// </summary>
            public string WorkingDirectoryRegistry { get; set; }
        }
    }
}
