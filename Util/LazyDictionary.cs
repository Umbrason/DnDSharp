namespace DnDSharp
{
    public class LazyDictionary<K, V> : Dictionary<K, V> where K : notnull where V : new()
    {
        public new V this[K key]
        {
            get
            {
                if (!ContainsKey(key))
                    Add(key, new());
                return base[key];
            }
            set
            {
                base[key] = value;
            }
        }
    }
}