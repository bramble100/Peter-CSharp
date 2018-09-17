using System;
using System.Collections.Generic;

namespace DataVendor.Models
{
    public class Isins
    {
        /// <summary>
        /// Key: Company name; Value: ISIN
        /// </summary>
        private Dictionary<string, string> _isins = new Dictionary<string, string>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public Isins()
        {
        }

        public int Count => _isins.Count;

        public string this[string name] => _isins[name];

        public bool ContainsKey(string name) => _isins.ContainsKey(name);

        public void Add(string[] input)
        {
            try
            {
                var name = string.IsNullOrWhiteSpace(input[0])
                    ? throw new ArgumentException($"ISIN cannot be added (name is null or empty).")
                    : input[0];
                var isin = input[1];

                if (!_isins.ContainsKey(name))
                {
                    _isins.Add(name, isin);
                }
                else
                {
                    _isins[name] = isin;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); ;
            }
        }
    }
}
