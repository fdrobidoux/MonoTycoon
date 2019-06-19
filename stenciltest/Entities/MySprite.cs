using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace stenciltest.Entities
{
	public class MySprite : DrawableGameComponent
	{
		Texture2D texture;

		public MySprite(Game game) : base(game)
		{

		}

		public override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			texture = Game.Content.Load<Texture2D>("sprite");
		}

		public override void Update(GameTime gt)
		{
			
		}

		public override void Draw(GameTime gt)
		{

		}
	}
}
