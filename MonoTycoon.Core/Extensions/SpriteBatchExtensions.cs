using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoTycoon.Core.Physics;

namespace MonoTycoon.Core
{
    public static class SpriteBatchExtensions
    {
        //public static void Draw(this SpriteBatch sb, Texture2D texture2D)
        public static void Draw(this SpriteBatch sb, Texture2D texture, Transform2 transform)
        {
            sb.Draw(texture, transform, transform.Center());
        }

        public static void Draw(this SpriteBatch sb, Texture2D texture, Transform2 transform, Vector2 origin)
        {
            sb.Draw(texture, transform.ToRectangle(), sourceRectangle: null, Color.White, transform.Rotation.Value, origin, SpriteEffects.None, transform.ZIndex);
        }
    }

    public enum OriginPrefab
    {
        ZERO,
        CENTER
    }
}