using DataVendor.Models;
using HtmlAgilityPack;
using NLog;
using Peter.Models.Builders;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DataVendor.Services.Html
{
    internal static class HtmlProcessor
    {
        internal static IEnumerable<IMarketDataEntity> GetMarketDataEntities(this StockExchangesHtmls stockExchangesHtmls) =>
            new HashSet<IMarketDataEntity>(
                stockExchangesHtmls.SelectMany(keyValuePair =>
                    GetTable(keyValuePair.Value)
                        .GetRows()
                        .GetMarketDataEntities(keyValuePair.Key)));

        internal static IEnumerable<IMarketDataEntity> GetMarketDataEntities(
            this IEnumerable<HtmlNode> rows,
            string stockExchangeName)
        {
            IEnumerable<IMarketDataEntity> entities = new List<IMarketDataEntity>(rows.Select(row => GetMarketDataEntity(row, stockExchangeName)));
            LogManager.GetCurrentClassLogger().Info($"Number of records added from {stockExchangeName}: {entities.Count()}");
            return entities.ToImmutableList();
        }

        internal static IMarketDataEntity GetMarketDataEntity(HtmlNode htmlTableRow, string stockExchange) =>
            new MarketDataEntityBuilder()
                .SetName(HtmlRowProcessor.GetName(htmlTableRow))
                .SetClosingPrice(HtmlRowProcessor.GetClosingPrice(htmlTableRow))
                .SetDateTime(HtmlRowProcessor.GetDateTime(htmlTableRow))
                .SetVolumen(HtmlRowProcessor.GetVolumen(htmlTableRow))
                .SetPreviousDayClosingPrice(HtmlRowProcessor.GetPreviousDayClosingPrice(htmlTableRow))
                .SetStockExchange(stockExchange)
                .Build();

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
