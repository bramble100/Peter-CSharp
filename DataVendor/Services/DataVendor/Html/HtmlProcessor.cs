using HtmlAgilityPack;
using Peter.Models.Builders;
using Peter.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Services.DataVendor.Html
{
    internal static class HtmlProcessor
    {
        internal static IEnumerable<IMarketDataEntity> GetMarketDataEntities(string htmlContent, string stockExchangeName) =>
            GetTable(htmlContent)
                .GetRows()
                .Select(row => GetMarketDataEntity(row, stockExchangeName))
                .Distinct();

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
