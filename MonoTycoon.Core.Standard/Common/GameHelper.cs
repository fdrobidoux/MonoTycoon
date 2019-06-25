using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoTycoon
{
    public class GameHelper
    {
        public static Game Game { get; private set; }
		public static SpriteBatch WorldBatch { get; internal set; }
		public static SpriteBatch UIBatch { get; internal set; }

		internal static void Inject(Game game)
            => Game = game;
    }
}
