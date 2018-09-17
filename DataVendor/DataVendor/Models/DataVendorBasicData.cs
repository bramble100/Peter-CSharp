using System;
using System.Collections.Generic;
using System.Linq;

namespace DataVendor.Models
{
    internal static class DataVendorBasicData
    {
        internal static Dictionary<string, Uri> Links => Environment
                    .GetEnvironmentVariable("PeterDataVendorLinks", EnvironmentVariableTarget.User)
                    .Split(';')
                    .Select(item => GetUriKeyValuePair(item))
                    .ToDictionary(item => item.Key, item => item.Value);

        private static KeyValuePair<string, Uri> GetUriKeyValuePair(string input)
        {
            var delimiterPosition = input.IndexOf('=');
            return new KeyValuePair<string, Uri>(input.Substring(0, delimiterPosition), new Uri(input.Substring(delimiterPosition + 1)));
        }
    }
}
