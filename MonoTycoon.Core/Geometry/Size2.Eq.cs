using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.Geometry
{
    public partial struct Size2 : IEquatable<Size2>, IEquatableByRef<Size2>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            switch (obj)
            {
                case Size2 size2: return Equals(size2);
                case Rectangle rectangle: return Equals(rectangle);
                case Point point: return Equals(point);
                default: return false;
            }
        }

        public bool Equals(Size2 other) => Equals(ref other);

        public bool Equals(ref Size2 other) 
            => Width == other.Width && Height == other.Height;

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }
    }
}