using System.Collections.Generic;

namespace MonoTycoon.Common
{
    public class DictionaryWithDefaults<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private readonly TValue _defaultValue;

        public DictionaryWithDefaults() : this(default) { }
        
        public DictionaryWithDefaults(TValue defaultValue) : base()
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