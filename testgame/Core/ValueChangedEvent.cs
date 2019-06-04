using System;
using System.Collections.Generic;
using MonoTycoon.Core.Common;

namespace testgame.Core
{
    public class ValueChangedEvent<T> where T : struct, IConvertible 
    {
        private DictionaryWithDefaults<KeyValuePair<T, T>, bool> _hasChangedDict;
        
        public T Previous { get; }
        public T Current { get; }

        public ValueChangedEvent(T old, T modified)
        {
            Previous = old;
            Current = modified;
        }

        public bool HasChangedFrom(T old, T modified) 
            => Was(old) && IsNow(modified);

        public bool Was(T old) 
            => Previous.Equals(old);

        public bool IsNow(T modified) 
            => Current.Equals(modified);
    }
}