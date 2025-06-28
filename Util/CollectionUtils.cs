namespace DnDSharp
{
    public static class CollectionUtils
    {
        public static V GetOrAdd<K, V>(this IDictionary<K, V> dictionary, K key, Func<V>? creator = null)
        {
            if (dictionary.TryGetValue(key, out var value)) return value;
            return dictionary[key] = (creator ?? Activator.CreateInstance<V>).Invoke();
        }
        public static void AddGroupElement<K, V, C>(this IDictionary<K, C> dictionary, K key, V value) where C : ICollection<V>
        {
            var group = dictionary.GetOrAdd(key);
            group.Add(value);
        }

        public static void RemoveGroupElement<K, V, C>(this IDictionary<K, ICollection<V>> dictionary, K key, V value) where C : ICollection<V>
        {
            if (!dictionary.ContainsKey(key)) return;
            var group = dictionary.GetOrAdd(key);
            group.Remove(value);
            if (group.Count == 0)
                dictionary.Remove(key);
        }
    }
}