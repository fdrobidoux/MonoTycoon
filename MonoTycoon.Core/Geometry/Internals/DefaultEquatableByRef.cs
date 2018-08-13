using System;

namespace MonoTycoon.Core.Geometry.Internals
{
    internal abstract class DefaultEquatableByRef<T> : IEquatable<T>, IEquatableByRef<T>
    {
        public bool Equals(T other) => Equals(ref other);
        public abstract bool Equals(ref T other);
    }
}