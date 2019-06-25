using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon;

namespace TryExtended.Components
{
	public class MovingCharacter : DrawableGameComponent
	{
		public Vector2 Position { get; set; }
		private Texture2D texture;

		public MovingCharacter(Game game) : base(game)
		{
			Position = Vector2.Zero;
		}

		public override void Initialize()
		{
			Position = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			texture = Game.Content.Load<Texture2D>("textures/speechless sonic");
		}

		public override void Update(GameTime gt)
		{

		}

		public override void Draw(GameTime gt)
		{
			GameHelper.WorldBatch.Draw(texture, Position, Color.White);
		}
	}
}
