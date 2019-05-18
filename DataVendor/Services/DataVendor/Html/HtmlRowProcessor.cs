using HtmlAgilityPack;
using System;
using System.Globalization;

namespace Services.DataVendor.Html
{
    internal static class HtmlRowProcessor
    {
        private static readonly CultureInfo huCulture = new CultureInfo("hu-HU");

        internal static string GetName(HtmlNode node) => 
            node
                .ChildNodes[0]
                .Attributes["title"]
                .Value;

        internal static decimal GetClosingPrice(HtmlNode node) => 
            Convert.ToDecimal(node
                .ChildNodes[1]
                .ChildNodes[0]
                .ChildNodes[0]
                .InnerText,
                huCulture);

        internal static DateTime GetDateTime(HtmlNode node) =>
            DateTime.ParseExact(node
                .ChildNodes[5]
                .ChildNodes[0]
                .ChildNodes[0]
                .InnerText,
                @"MM.dd./HH:mm",
                CultureInfo.InvariantCulture);

        internal static int GetVolumen(HtmlNode node) =>
            Convert.ToInt32(node
                .ChildNodes[6]
                .ChildNodes[0]
                .ChildNodes[0]
                .InnerText);

        internal static decimal GetPreviousDayClosingPrice(HtmlNode node) =>
            Convert.ToDecimal(node
                .ChildNodes[7]
                .ChildNodes[0]
                .ChildNodes[0]
                .InnerText,
                huCulture);
    }
}
