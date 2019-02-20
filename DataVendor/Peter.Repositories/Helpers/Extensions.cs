using System;
using System.Collections.Generic;

namespace Peter.Repositories.Helpers
{
    public static class Extensions
    {
        public static string FormatterForCSV(this KeyValuePair<string, string> nameToIsin, string separator)
        {
            return string.Join(separator,
                nameToIsin.Key.WrapWithQuotes(),
                nameToIsin.Value);
        }

        public static string WrapWithQuotes(this object obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            return $"\"{obj.ToString()}\"";
        }
    }
}
