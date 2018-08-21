using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace testgame.Core
{
    public interface IActuallyDrawable
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}