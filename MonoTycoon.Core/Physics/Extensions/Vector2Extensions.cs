using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.Physics
{
    public static class Vector2Extensions
    {
        internal static List<Action> callbacks { get; private set; } = new List<Action>();

        public static float GetDistance(float speed, TimeSpan deltaTime)
            => (float)(speed * deltaTime.TotalMilliseconds);

        public static Vector2 GetDirectionTowards(this Vector2 source, Vector2 destination)
        {
            var direction = new Vector2(destination.X - source.X, destination.Y - source.Y);
            direction.Normalize();
            if (float.IsNaN(direction.X) || float.IsNaN(direction.Y))
                return Vector2.Zero;
            return direction;
        }

        public static Vector2 GetLocation(this Vector2 source, Vector2 direction, float distance)
        {
            var part1 = new Vector2(direction.X * distance, direction.Y * distance);
            return new Vector2(source.X + part1.X, source.Y + part1.Y);
        }

        public static float GetDistanceBetween(this Vector2 source, Vector2 destination)
        {
            var distance = new Vector2(source.X - destination.X, source.Y - destination.Y).Length();
            return float.IsNaN(distance) ? 1 : distance;
        }

        public static void MoveTowards(this Vector2 source, Vector2 destination, float speed, TimeSpan deltaTime, Action<Vector2> moveCallback)
            => MoveTowards(source, destination, GetDistance(speed, deltaTime), moveCallback);

        public static void MoveTowards(this Vector2 source, Vector2 destination, float distance, Action<Vector2> moveCallback)
            => Move(source, GetDirectionTowards(source, destination), Math.Min(distance, GetDistanceBetween(source, destination)), moveCallback);

        public static void Move(this Vector2 source, Vector2 direction, float speed, TimeSpan deltaTime, Action<Vector2> moveCallback)
            => Move(source, direction, GetDistance(speed, deltaTime), moveCallback);

        public static void Move(this Vector2 source, Vector2 direction, float distance, Action<Vector2> moveCallback)
            => callbacks.Add(() => moveCallback(GetLocation(source, direction, distance)));

        public static void Arrive(this Vector2 source, Vector2 destination, Action uponArrivial)
            => callbacks.Add(() =>
            {
                if (Math.Abs(source.X - destination.X) < 1 && Math.Abs(source.Y - destination.Y) < 1)
                    uponArrivial();
            });

        public static void Resolve()
        {
            var list = callbacks;
            callbacks = new List<Action>();
            list.ForEach(x => x());
        }
    }
}