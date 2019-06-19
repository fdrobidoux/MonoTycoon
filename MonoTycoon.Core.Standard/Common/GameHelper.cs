using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoTycoon.Common
{
    public class GameHelper
    {
        protected static Game Game { get; private set; }

        internal static void Inject(Game game)
            => Game = game;
    }
}
