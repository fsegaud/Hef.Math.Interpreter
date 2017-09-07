namespace Hef.Collection
{
    internal class Cache<TKey, TValue>
    {
        public delegate TValue InitializeValue(TKey key);

        private readonly System.Collections.Generic.Dictionary<TKey, CachedValue> cache = new System.Collections.Generic.Dictionary<TKey, CachedValue>();

        private int maxValues;

        public Cache(int maxValues = int.MaxValue)
        {
            this.maxValues = maxValues;
        }

        public int Count => this.cache.Count;

        public TValue GetOrInitializeValue(TKey key, InitializeValue initializeValue)
        {
            lock (cache)
            {
                CachedValue cachedValue;
                if (this.cache.TryGetValue(key, out cachedValue))
                {
                    return cachedValue.Value;
                }

                TValue value = initializeValue.Invoke(key);
                this.cache.Add(key, new CachedValue(value));

                if (this.cache.Count > this.maxValues)
                {
                    int minID = int.MaxValue;
                    TKey keyToRemove = default(TKey);
                    foreach (System.Collections.Generic.KeyValuePair<TKey, CachedValue> kvp in this.cache)
                    {
                        if (kvp.Value.ID < minID)
                        {
                            minID = kvp.Value.ID;
                            keyToRemove = kvp.Key;
                        }
                    }

                    this.cache.Remove(keyToRemove);
                }

                return value;
            }
        }

        public void Clear()
        {
            lock (cache)
            {
                this.cache.Clear();
            }
        }

        internal string Dump()
        {
            System.Text.StringBuilder dump = new System.Text.StringBuilder();

            lock (this.cache)
            {
                foreach (System.Collections.Generic.KeyValuePair<TKey, CachedValue> kvp in this.cache)
                {
                    dump.AppendFormat("[{0}] {1} => {2}\n", kvp.Value.ID, kvp.Key, kvp.Value.Value);
                }
            }

            return dump.ToString();
        }

        private struct CachedValue
        {
            private static int nextID = 0;

            public readonly int ID;
            public readonly TValue Value;

            public CachedValue(TValue value)
            {
                this.ID = CachedValue.nextID++;
                Value = value;
            }
        }
    }
}