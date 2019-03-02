using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Services.DataVendor.Models
{
    /// <summary>
    /// Stores the downloaded html contents by the name of the stock exchanges.
    /// </summary>
    internal class StockExchangesHtmls
    {
        /// <summary>
        /// Stores the HTML string by stock exchange link.
        /// </summary>
        private readonly Dictionary<string, string> _htmls = new Dictionary<string, string>();

        /// <summary>
        /// Constructor.
        /// </summary>
        internal StockExchangesHtmls()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerable"></param>
        internal StockExchangesHtmls(IEnumerable<KeyValuePair<string, string>> enumerable) : this()
        {
            enumerable.ToList().ForEach(Add);
        }

        /// <summary>
        /// Indexer by name of the stock exchange.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal string this[string key]
        {
            get => _htmls[key];
            set => _htmls[key] = value;
        }

        internal IEnumerable<string> Keys => _htmls.Keys;

        /// <summary>
        /// Adds a html content to the collection.
        /// </summary>
        /// <param name="stockExchangeName"></param>
        /// <param name="html"></param>
        /// <param name="forceOverwrite"></param>
        /// <param name="minifyFunction"></param>
        internal void Add(
            string stockExchangeName,
            string html,
            bool forceOverwrite = false,
            Func<string,string> minifyFunction = null)
        {
            if (string.IsNullOrEmpty(nameof(stockExchangeName)))
                throw new ArgumentNullException(nameof(stockExchangeName));
            if (string.IsNullOrEmpty(html))
                throw new ArgumentNullException(nameof(html));
            if (!forceOverwrite && _htmls.ContainsKey(stockExchangeName))
                throw new InvalidOperationException(
                    "The given stock exchange name already exists in collection (you might try force overwrite).");

            if (!_htmls.ContainsKey(stockExchangeName))
                _htmls.Add(stockExchangeName, string.Empty);

            if (minifyFunction != null)
                html = minifyFunction(html);
            _htmls[stockExchangeName] = html;
        }

        /// <summary>
        /// Projects each element of a sequence to an IEnumerable and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        internal IEnumerable<IMarketDataEntity> SelectMany(
            Func<KeyValuePair<string, string>, IEnumerable<IMarketDataEntity>> function)
        {
            return _htmls.SelectMany(keyValuePair => function(keyValuePair));
        }

        private void Add(KeyValuePair<string, string> kvp) => _htmls.Add(kvp.Key, kvp.Value);

        private static string MinifyLinq(string input) =>
            new string(input
                .Where(c => !char.IsWhiteSpace(c) || Equals(c, ' '))
                .ToArray());

        private static string MinifyRegex(string input)
        {
            Regex.Replace(input, @"\s+", "");
            return input;
        }
    }
}
