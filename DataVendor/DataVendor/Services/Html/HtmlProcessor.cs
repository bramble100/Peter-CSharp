using DataVendor.Models;
using HtmlAgilityPack;
using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

// http://html-agility-pack.net/from-string

namespace DataVendor.Services.Html
{
    internal static class HtmlProcessor
    {
        internal static IMarketDataEntities GetMarketDataEntities(this StockExchangesHtmls stockExchangesHtmls) =>
            new MarketDataEntities(
                stockExchangesHtmls.SelectMany(keyValuePair =>
                    GetTable(keyValuePair.Value)
                        .GetRows()
                        .GetMarketDataEntities(keyValuePair.Key)));

        internal static IMarketDataEntities GetMarketDataEntities(
            this IEnumerable<HtmlNode> rows,
            string stockExchangeName)
        {
            IMarketDataEntities entities = new MarketDataEntities(rows.Select(row => GetMarketDataEntity(row, stockExchangeName)));
            LogManager.GetCurrentClassLogger().Info($"Number of records added from {stockExchangeName}: {entities.Count}");
            return entities;
        }

        internal static IMarketDataEntity GetMarketDataEntity(HtmlNode htmlTableRow, string stockExchange) =>
            new MarketDataEntity
            {
                Name = HtmlRowProcessor.GetName(htmlTableRow),
                ClosingPrice = HtmlRowProcessor.GetClosingPrice(htmlTableRow),
                DateTime = HtmlRowProcessor.GetDateTime(htmlTableRow),
                Volumen = HtmlRowProcessor.GetVolumen(htmlTableRow),
                PreviousDayClosingPrice = HtmlRowProcessor.GetPreviousDayClosingPrice(htmlTableRow),
                StockExchange = stockExchange
            };

        internal static HtmlNode GetTable(string htmlString)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);

            return htmlDoc
                .DocumentNode
                .Descendants()
                .Where(n => string.Equals(n.Name, "table"))
                .ToList()[1];
        }

        internal static IEnumerable<HtmlNode> GetRows(this HtmlNode htmlTable) =>
            htmlTable
                .Descendants()
                .Where(n => string.Equals(n.Name, "tr"))
                .Skip(3);
    }
}
