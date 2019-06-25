using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Structures;

namespace MonoTycoon
{
    public class TycoonGame : Game
    {
		public TycoonGame() : base()
        {
            GameHelper.Inject(this);
		}

		internal static void LoadInjectables(TycoonGame game)
		{
			//ServiceInjector.AddService(new);
		}

		protected override void Initialize()
        {
            // Initialize systems.
            base.Initialize();
		}

		protected override void LoadContent()
		{
			GameHelper.WorldBatch = new SpriteBatch(GraphicsDevice);
			GameHelper.UIBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}
	}
}
