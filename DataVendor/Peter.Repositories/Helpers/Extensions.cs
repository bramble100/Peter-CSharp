using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

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

        public static string WrapWithQuotes(this object obj) => $"\"{obj.ToString()}\"";
    }
}
