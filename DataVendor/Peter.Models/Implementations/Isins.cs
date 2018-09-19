using Peter.Models.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Peter.Models.Implementations
{
    public class Isins : IIsins
    {
        private Dictionary<string, string> _isins;

        public Isins()
        {
            _isins = new Dictionary<string, string>();
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

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)_isins).GetEnumerator();
        }

        public void Remove(KeyValuePair<string, string> dn) => _isins.Remove(dn.Key);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)_isins).GetEnumerator();
        }

        public void Add(KeyValuePair<string, string> keyValuePair)
        {
            if (string.IsNullOrWhiteSpace(keyValuePair.Key)) return;

            _isins.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}
