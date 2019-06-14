using SDPoint = System.Drawing.Point;
using XNAPoint = Microsoft.Xna.Framework.Point;
using Microsoft.Xna.Framework;
using System;

namespace MonoTycoon.Core.Physics
{
    public static class PointExtensions
    {
        public static Point Scale(this Point point, float scale)
            => (point.ToVector2() * scale).ToPoint();

        public static Point DivideBy(this Point thisPoint, int division)
        {
            int _x = (int)Math.Round(thisPoint.X / (double)division, 0);
            thisPoint.X = _x;

            int _y = (int)Math.Round(thisPoint.Y / (double)division, 0);
            thisPoint.Y = _y;

            return thisPoint;
        }

        public static Point Scale(this Point point, double scale)
        {
            return (point.ToVector2() * (float)scale).ToPoint();
        }

        public static Size2 ToSize2(this SDPoint point)
            => new Size2(point.X, point.Y);

        public static Size2 ToSize2(this XNAPoint point)
            => new Size2(point.X, point.Y);


    }
}