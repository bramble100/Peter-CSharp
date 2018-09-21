using Peter.Models.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace Peter.Models.Implementations
{
    public class Registry : IRegistry
    {
        private IRegistryEntry _entries;

        public IRegistryEntry this[string key] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public ICollection<string> Keys => throw new System.NotImplementedException();

        public ICollection<IRegistryEntry> Values => throw new System.NotImplementedException();

        public int Count => throw new System.NotImplementedException();

        public bool IsReadOnly => throw new System.NotImplementedException();

        public void Add(string key, IRegistryEntry value)
        {
            throw new System.NotImplementedException();
        }

        public void Add(KeyValuePair<string, IRegistryEntry> item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, IRegistryEntry> item)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, IRegistryEntry>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, IRegistryEntry>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, IRegistryEntry> item)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(string key, out IRegistryEntry value)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
