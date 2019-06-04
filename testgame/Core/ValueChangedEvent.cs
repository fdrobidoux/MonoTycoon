using System;
using System.Collections.Generic;
using MonoTycoon.Core.Common;

namespace testgame.Core
{
    public class ValueChangedEvent<T> where T : struct, IConvertible 
    {
        private DictionaryWithDefaults<KeyValuePair<T, T>, bool> _hasChangedDict;
        
        public T Old { get; }
        public T Modified { get; }

        public ValueChangedEvent(T old, T modified)
        {
            Old = old;
            Modified = modified;
        }

        public bool HasChangedFrom(T old, T modified) 
            => Was(old) && IsNow(modified);

        public bool Was(T old) 
            => Old.Equals(old);

        public bool IsNow(T modified) 
            => Modified.Equals(modified);
    }
}