using System;
using Microsoft.Xna.Framework;
using MonoTycoon.Structures;

namespace MonoTycoon
{
    public class TycoonGame : Game
    {
        // TODO: Code `TycoonGame` class.
        
        public TycoonGame() : base()
        {
            Common.GameHelper.Inject(this);
        }

		internal static void LoadInjectables(TycoonGame game)
		{
			ServiceInjector.AddService(new );
		}

		protected override void Initialize()
        {
            // Initialize systems.
            base.Initialize();
        }
    }
}
