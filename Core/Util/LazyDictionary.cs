using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DnDSharp
{
    public class LazyDictionary<K, V> : IDictionary<K, V>, IReadOnlyDictionary<K, V> where K : notnull where V : new()
    {
        private readonly IDictionary<K, V> internalDictionary = new Dictionary<K, V>();
        public V this[K key]
        {
            get
            {
                if (!internalDictionary.ContainsKey(key))
                    internalDictionary[key] = new();
                return internalDictionary[key];
            }
            set
            {
                internalDictionary[key] = value;
            }
        }
        public ICollection<K> Keys => internalDictionary.Keys;
        public ICollection<V> Values => internalDictionary.Values;
        public int Count => internalDictionary.Count;
        public bool IsReadOnly => false;

        IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => internalDictionary.Keys;
        IEnumerable<V> IReadOnlyDictionary<K, V>.Values => internalDictionary.Values;

        public void Add(K key, V value)
        {
            internalDictionary.Add(key, value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            internalDictionary.Add(item);
        }

        public void Clear()
        {
            internalDictionary.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return internalDictionary.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return internalDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            internalDictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return internalDictionary.GetEnumerator();
        }

        public bool Remove(K key)
        {
            return internalDictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return internalDictionary.Remove(item);
        }

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            return internalDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)internalDictionary).GetEnumerator();
        }
    }
}