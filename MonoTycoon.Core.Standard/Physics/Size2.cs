using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoTycoon.Core.Physics
{
    public struct Size2
	{
        public static readonly Size2 Zero = new Size2(0, 0);

        public int Width { get; }
        public int Height { get; }

        public Size2(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{Width}, {Height}";
        }

        public Point ToPoint()
        {
            return new Point(Width, Height);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(Width, Height);
        }

		public static Size2 operator +(Size2 s1, Size2 s2)
        {
            return new Size2(s1.Width + s2.Width, s1.Height + s2.Height);
        }

        public static Size2 operator -(Size2 s1, Size2 s2)
        {
            return new Size2(s1.Width - s2.Width, s1.Height - s2.Height);
        }

        public static Size2 operator *(Size2 size, float scale)
        {
            return new Size2((int) (size.Width * scale), (int) (size.Height * scale));
        }

        public static Size2 operator /(Size2 size, float scale)
        {
            return new Size2((int)(size.Width / scale), (int)(size.Height / scale));
        }

        public static Size2 operator *(Size2 size, Vector2 scale)
        {
            return new Size2((int)(size.Width * scale.X), (int)(size.Height * scale.Y));
        }

        public static Size2 operator *(Vector2 scale, Size2 size)
        {
            return size * scale;
        }

        public static Size2 operator -(Point point, Size2 size)
        {
            return new Size2(point.X - size.Width, point.Y - size.Height);
        }

        public static Size2 operator -(Size2 size, Point point)
        {
            return new Size2(size.Width - point.X, size.Height - point.Y);
        }
    }
}
