using DataVendor.Models;
using HtmlAgilityPack;
using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

// http://html-agility-pack.net/from-string

namespace DataVendor.Services.Html
{
    internal static class HtmlProcessor
    {
        internal static IEnumerable<IMarketDataEntity> GetMarketDataEntities(this StockExchangesHtmls stockExchangesHtmls)
        {
            return new List<IMarketDataEntity>(
                stockExchangesHtmls.SelectMany(keyValuePair =>
                    GetTable(keyValuePair.Value)
                    .GetRows()
                    .GetMarketDataEntities(keyValuePair.Key))).ToImmutableList();
        }

        internal static IEnumerable<IMarketDataEntity> GetMarketDataEntities(
            this IEnumerable<HtmlNode> rows, 
            string stockExchangeName)
        {
            IEnumerable<IMarketDataEntity> entities = new List<IMarketDataEntity>(rows.Select(row => GetMarketDataEntity(row, stockExchangeName)));
            LogManager.GetCurrentClassLogger().Info($"Number of records added from {stockExchangeName}: {entities.Count()}");
            return entities.ToImmutableList();
        }

        internal static IMarketDataEntity GetMarketDataEntity(HtmlNode htmlTableRow, string stockExchange)
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
                .Where(n => string.Equals(n.Name, "table"))
                .ToList()[1];
        }

        internal static IEnumerable<HtmlNode> GetRows(this HtmlNode htmlTable)
        {
            return htmlTable
                .Descendants()
                .Where(n => string.Equals(n.Name, "tr"))
                .Skip(3);
        }
    }
}
