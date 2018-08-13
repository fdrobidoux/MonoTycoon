using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.Geometry
{
    public partial struct Size2 : IEquatable<Point>, IEquatableByRef<Point>
    {
        public Size2(Point point)
        {
            Width = point.X;
            Height = point.Y;
        }

        public bool Equals(Point other) => Equals(ref other);
        
        public bool Equals(ref Point other) 
            => (Width == other.X) && (Height == other.Y);

        public static implicit operator Size2(Point point) 
            => new Size2(point);

        public static explicit operator Point(Size2 size) 
            => new Point(size.Width, size.Height);
    }

    public static partial class PointExtensions
    {
        public static Size2 ToSize2(this Point _this)
            => new Size2(_this);
    }
}