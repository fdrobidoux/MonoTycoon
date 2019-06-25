using System;
using System.Collections.Generic;
using MonoTycoon;

namespace Pong.Core
{
    public class ValueChangedEvent<T> : EventArgs 
		where T : struct, IConvertible
    {
        //private DictionaryWithDefaults<KeyValuePair<T, T>, bool> _hasChangedDict;
        
        public T Previous { get; }
        public T Current { get; }

        public ValueChangedEvent(T prev, T curr)
        {
            Previous = prev;
            Current = curr;
        }

		public ValueChangedEvent() : base()
		{ }

		public bool HasChangedFrom(T prev, T curr) 
            => Was(prev) && IsNow(curr);

        public bool Was(T prev) 
            => Previous.Equals(prev);

        public bool IsNow(T curr) 
            => Current.Equals(curr);
    }
}