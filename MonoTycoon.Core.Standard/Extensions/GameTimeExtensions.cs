using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework
{
    public static class GameTimeExtensions
    {
        /// <summary>
        /// Returns the common delta (seconds elapsed 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>Total seconds since last frame/update.</returns>
        public static float Delta(this GameTime theGameTime) => (float) theGameTime.ElapsedGameTime.TotalSeconds;
    }
}