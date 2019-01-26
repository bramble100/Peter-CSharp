using NLog;
using Peter.Models.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace Peter.Models.Implementations
{
    public class NameToIsins : INameToIsins
    {
        private Dictionary<string, string> _isins;

        public NameToIsins()
        {
            _isins = new Dictionary<string, string>();
        }

        public int Count => _isins.Count;

        public string this[string name] => _isins[name];

        public void Add(string[] input)
        {
            if (Validators.NameToIsin.TryParse(input, out var name, out var isin))
            {
                Add(name, isin);
            }
            else
            {
                LogManager.GetCurrentClassLogger().Warn($"ISIN cannot be added (name is null or empty).");
            }
        }

        public void Add(KeyValuePair<string, string> keyValuePair)
        {
            if (Validators.NameToIsin.TryParse(keyValuePair, out var name, out var isin))
            {
                Add(name, isin);
            }
            else
            {
                LogManager.GetCurrentClassLogger().Warn($"ISIN cannot be added (name is null or empty).");
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

        public bool ContainsKey(string name) => _isins.ContainsKey(name);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => ((IEnumerable<KeyValuePair<string, string>>)_isins).GetEnumerator();

        public void Remove(KeyValuePair<string, string> nameToIsin) => _isins.Remove(nameToIsin.Key);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<string, string>>)_isins).GetEnumerator();
    }
}
