using DataVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DataVendor.Services
{
    internal static class HtmlDownloader
    {
        internal static StockExchangesHtmls DownloadAll()
        {
            using (var client = new WebClient())
            {
                return new StockExchangesHtmls(
                    DataVendorBasicData
                        .Links
                        .Select(link => new KeyValuePair<string, string>(
                            link.Key,
                            Encoding.UTF8.GetString(Download(link, client)))));
            }
        }

        private static byte[] Download(KeyValuePair<string, Uri> link, WebClient client)
        {
            Console.WriteLine($"Downloading: {link.Key}");
            return client.DownloadData(link.Value);
        }
    }
}
