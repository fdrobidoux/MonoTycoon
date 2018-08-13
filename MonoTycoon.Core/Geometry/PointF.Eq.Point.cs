using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.Geometry
{
    public partial struct PointF : IEquatable<Point>, IEquatableByRef<Point>
    {
        public bool Equals(Point other) => Equals(ref other);
        public bool Equals(ref Point other) 
            => this.X.Equals(other.X) && this.Y.Equals(other.Y);
    }
}