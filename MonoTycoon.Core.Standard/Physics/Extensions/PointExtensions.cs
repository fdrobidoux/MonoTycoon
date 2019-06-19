#if !LITE
using SDPoint = System.Drawing.Point;
#endif
using System;
using Microsoft.Xna.Framework;
using XNAPoint = Microsoft.Xna.Framework.Point;

namespace MonoTycoon.Physics
{
    public static class PointExtensions
    {
        public static XNAPoint Scale(this XNAPoint point, float scale)
            => (point.ToVector2() * scale).ToPoint();

        public static XNAPoint DivideBy(this XNAPoint thisPoint, int division)
        {
            int _x = (int)Math.Round(thisPoint.X / (double)division, 0);
            thisPoint.X = _x;

            int _y = (int)Math.Round(thisPoint.Y / (double)division, 0);
            thisPoint.Y = _y;

            return thisPoint;
        }
		
        public static XNAPoint Scale(this XNAPoint point, double scale)
        {
            return (point.ToVector2() * (float)scale).ToPoint();
        }

#if !LITE
        public static Size2 ToSize2(this SDPoint point)
            => new Size2(point.X, point.Y);
#endif
		public static Size2 ToSize2(this XNAPoint point)
            => new Size2(point.X, point.Y);
    }
}