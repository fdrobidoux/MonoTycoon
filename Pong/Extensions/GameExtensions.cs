using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public static class GameExtensions
    {
        public static SpriteBatch GetSpriteBatch(this Game game)
        {
            return Game1.SpriteBatch;
        }


    }
}