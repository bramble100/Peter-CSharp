using HtmlAgilityPack;
using System;
using System.Globalization;

namespace DataVendor.Services.Html
{
    internal static class HtmlRowProcessor
    {

        internal static string GetName(HtmlNode node)
        {
            return node
                .ChildNodes[0]
                .Attributes["title"]
                .Value;
        }

        internal static decimal GetClosingPrice(HtmlNode node)
        {
            return Convert.ToDecimal(node
                .ChildNodes[1]
                .ChildNodes[0]
                .ChildNodes[0]
                .InnerText, 
                new CultureInfo("hu-HU"));
        }

        internal static DateTime GetDateTime(HtmlNode node)
        {
            return DateTime.ParseExact(node
                .ChildNodes[5]
                .ChildNodes[0]
                .ChildNodes[0]
                .InnerText,
                @"MM.dd./HH:mm",
                CultureInfo.InvariantCulture);
        }

        internal static int GetVolumen(HtmlNode node)
        {
            return Convert.ToInt32(node
                .ChildNodes[6]
                .ChildNodes[0]
                .ChildNodes[0]
                .InnerText);
        }

        internal static decimal GetPreviousDayClosingPrice(HtmlNode node)
        {
            return Convert.ToDecimal(node
                .ChildNodes[7]
                .ChildNodes[0]
                .ChildNodes[0]
                .InnerText,
                new CultureInfo("hu-HU"));
        }
    }
}
