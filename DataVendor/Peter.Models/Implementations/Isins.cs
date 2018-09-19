using Peter.Models.Interfaces;
using Peter.Models.Validators;
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
            if (Isin.TryParse(input, out var name, out var isin))
            {
                Add(name, isin);
            }
            else
            {
                // TODO: log
                Console.WriteLine($"ISIN cannot be added (name is null or empty).");
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
            if (Isin.TryParse(keyValuePair, out var name, out var isin))
            {
                Add(name, isin);
            }
            else
            {
                // TODO: log
                Console.WriteLine($"ISIN cannot be added (name is null or empty).");
            }
        }

        private void Add(string name, string isin)
        {
            if (!_isins.ContainsKey(name))
            {
                _isins.Add(name, isin);
            }
            else
            {
                _isins[name] = isin;
            }
        }
    }
}
