using DataVendor.Models;
using HtmlAgilityPack;
using Peter.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

// http://html-agility-pack.net/from-string

namespace DataVendor.Services
{
    internal static class HtmlProcessor
    {
        internal static MarketDataEntities GetMarketDataEntities(this StockExchangesHtmls stockExchangesHtmls)
        {
            return new MarketDataEntities(
                stockExchangesHtmls.SelectMany(keyValuePair =>
                    GetTable(keyValuePair.Value)
                    .GetRows()
                    .GetMarketDataEntities(keyValuePair.Key)));
        }

        internal static MarketDataEntities GetMarketDataEntities(
            this IEnumerable<HtmlNode> rows, 
            string stockExchangeName)
        {
            var entities = new MarketDataEntities(rows.Select(row => GetMarketDataEntity(row, stockExchangeName)));
            Console.WriteLine($"Number of records added from {stockExchangeName}: {entities.Count}");
            return entities;
        }

        internal static MarketDataEntity GetMarketDataEntity(HtmlNode htmlTableRow, string stockExchange)
        {
            return new MarketDataEntity
            {
                Name = HtmlRowProcessor.GetName(htmlTableRow),
                ClosingPrice = HtmlRowProcessor.GetClosingPrice(htmlTableRow),
                DateTime = HtmlRowProcessor.GetDateTime(htmlTableRow),
                Volumen = HtmlRowProcessor.GetVolumen(htmlTableRow),
                PreviousDayClosingPrice = HtmlRowProcessor.GetPreviousDayClosingPrice(htmlTableRow),
                StockExchange = stockExchange
            };
        }

        internal static HtmlNode GetTable(string htmlString)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);

            return htmlDoc
                .DocumentNode
                .Descendants()
                .Where(n => String.Equals(n.Name, "table"))
                .ToList()[1];
        }

        internal static IEnumerable<HtmlNode> GetRows(this HtmlNode htmlTable)
        {
            return htmlTable
                .Descendants()
                .Where(n => String.Equals(n.Name, "tr"))
                .Skip(3);
        }
    }
}
