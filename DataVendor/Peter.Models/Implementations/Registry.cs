using Peter.Models.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace Peter.Models.Implementations
{
    public class Registry : IRegistry
    {
        private Dictionary<string, IRegistryEntry> _entries;

        public Registry()
        {
            _entries = new Dictionary<string, IRegistryEntry>();
        }

        public Registry(IEnumerable<KeyValuePair<string, IRegistryEntry>> keyValuePairs) : this()
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                _entries.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public IRegistryEntry this[string key]
        {
            get => _entries[key];
            set => _entries[key] = value;
        }

        public ICollection<string> Keys => _entries.Keys;

        public ICollection<IRegistryEntry> Values => _entries.Values;

        public int Count => _entries.Count;

        public bool IsReadOnly => false;

        public void Add(string key, IRegistryEntry value)
        {
            if (!_entries.ContainsKey(key))
            {
                _entries.Add(key, value);
            }
            else
            {
                _entries[key] = value;
            }
        }

        public void Add(KeyValuePair<string, IRegistryEntry> item) => Add(item.Key, item.Value);

        public void Clear() => _entries.Clear();

        public bool Contains(KeyValuePair<string, IRegistryEntry> item) => _entries.ContainsKey(item.Key) && _entries.ContainsValue(item.Value);

        public bool ContainsKey(string key) => _entries.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, IRegistryEntry>[] array, int arrayIndex) => throw new System.NotImplementedException();

        public IEnumerator<KeyValuePair<string, IRegistryEntry>> GetEnumerator() => _entries.GetEnumerator();

        public bool Remove(string key) => _entries.Remove(key);

        public bool Remove(KeyValuePair<string, IRegistryEntry> item) => _entries.Remove(item.Key);

        public override string ToString() => $"Registry with {_entries.Count} entries.";

        public bool TryGetValue(string key, out IRegistryEntry value) => _entries.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _entries.Keys.GetEnumerator();
    }
}
