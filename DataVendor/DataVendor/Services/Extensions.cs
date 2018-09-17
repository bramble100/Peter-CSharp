using System.Collections.Generic;

namespace DataVendor.Services
{
    internal static class Extensions
    {
        public static KeyValuePair<string, string> IsinParserFromCSV(this IEnumerable<string> strings)
        {
            var queue = new Queue<string>(strings);
            return new KeyValuePair<string, string>(queue.Dequeue(), queue.Dequeue());
        }
    }
}
