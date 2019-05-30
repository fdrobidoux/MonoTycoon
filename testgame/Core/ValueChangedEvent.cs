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
            _hasChangedDict = new DictionaryWithDefaults<KeyValuePair<T, T>, bool>(false)
            {
                {new KeyValuePair<T, T>(old, modified), true}
            };
        }

        public bool HasChangedFrom(T old, T modified) 
            => _hasChangedDict[new KeyValuePair<T, T>(old, modified)];
    }
}