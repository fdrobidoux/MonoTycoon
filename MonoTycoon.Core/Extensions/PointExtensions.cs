using System.Transactions;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.Extensions
{
    public static class PointExtensions
    {
        public static Point Scale(this Point point, float scale) 
            => (point.ToVector2() * scale).ToPoint();
    }
}