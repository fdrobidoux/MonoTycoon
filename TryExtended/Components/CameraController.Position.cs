using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace TryExtended.Components
{
    public partial class CameraController : DrawableGameComponent
    {
        public Vector2 direction;
        public float acceleration = 100f;

        private void UpdatePosition(GameTime gt, KeyboardState kb)
        {
            direction = Vector2.Zero;

            // Y Axis
            if (kb.IsKeyDown(Keys.Up))
                direction -= Vector2.UnitY;
            else if (kb.IsKeyDown(Keys.Down))
                direction += Vector2.UnitY;

            // X Axis
            if (kb.IsKeyDown(Keys.Left))
                direction -= Vector2.UnitX;
            else if (kb.IsKeyDown(Keys.Right))
                direction += Vector2.UnitX;

            Camera.Move(direction * (float)(acceleration * gt.ElapsedGameTime.TotalSeconds));
        }
    }
}
