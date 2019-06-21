using System.Collections.Concurrent;

namespace MonoTycoon
{
    public class ConcurrentDictionaryWithDefaults<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {
        private readonly TValue _defaultValue;
        
        public ConcurrentDictionaryWithDefaults(TValue defaultValue) : base()
        {
            _defaultValue = defaultValue;
        }

        public new TValue this[TKey key]
        {
            get => ContainsKey(key) ? base[key] : _defaultValue;
            set => base[key] = value;
        }
    }
}