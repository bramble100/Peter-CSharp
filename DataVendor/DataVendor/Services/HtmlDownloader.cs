using DataVendor.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DataVendor.Services
{
    internal static class HtmlDownloader
    {
        /// <summary>
        /// Downloads all datavendor pages and returns the contents of html files separately.
        /// </summary>
        /// <returns>The contents of html files separately</returns>
        internal static StockExchangesHtmls DownloadAll()
        {
            using (var client = new WebClient())
            {
                return new StockExchangesHtmls(
                    DataVendorBasicData
                        .Links
                        .Select(link => new KeyValuePair<string, string>(
                            link.Key,
                            Download(link.Key, link.Value, client))));
            }
        }

        /// <summary>
        /// Downloads a html page and returns the content as a string.
        /// </summary>
        /// <param name="name">The name of the page.</param>
        /// <param name="uri">The Uri of the page.</param>
        /// <param name="client">A Webclient instance.</param>
        /// <returns>The html content.</returns>
        private static string Download(string name, Uri uri, WebClient client)
        {
            LogManager.GetCurrentClassLogger().Info($"Downloading: {name}");
            return Encoding.UTF8.GetString(client.DownloadData(uri));
        }
    }
}
