using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.Geometry
{
    public partial struct Size2 : IEquatable<Rectangle>, IEquatableByRef<Rectangle>
    {
        public Size2(Rectangle rectangle)
        {
            Width = rectangle.Width;
            Height = rectangle.Height;
        }

        public bool Equals(Rectangle other) => Equals(ref other);

        public bool Equals(ref Rectangle other) 
            => this.Width == other.Width && this.Height == other.Width;

        public static implicit operator Size2(Rectangle rectangle) 
            => new Size2(rectangle);

        public static explicit operator Rectangle(Size2 size) 
            => new Rectangle(Point.Zero, (Point) size);
    }

    public static partial class RectangleExtensions
    {
        public static Size2 ToSize2(this Rectangle thisRect)
            => new Size2(thisRect);
    }
}