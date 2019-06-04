using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core
{
    public class TycoonGame : Game
    {
        // TODO: Code `TycoonGame` class.
        
        public TycoonGame() : base()
        {
            Common.GameHelper.Inject(this);
        }

        protected override void Initialize()
        {
            // Initialize systems.
            base.Initialize();
        }
    }
}
