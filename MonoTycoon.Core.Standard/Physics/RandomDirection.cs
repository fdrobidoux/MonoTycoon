using System;
using Microsoft.Xna.Framework;
using MonoTycoon.Common;

namespace MonoTycoon.Physics
{
    public class RandomDirection
    {
        private Vector2 _direction;
        private static readonly Random Random = new Random();

        public Vector2 Get()
        {
            if (_direction == Vector2.Zero)
                ResolveRandomDirection();
            return _direction;
        }

        private void ResolveRandomDirection()
        {
            _direction = Vector2.Zero.GetDirectionTowards(new Vector2(Random.Next(-100, 100), Random.Next(-100, 100)));
        }
    }
}
