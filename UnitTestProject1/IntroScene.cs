using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonoTycoon.Core.Screens;
using UnitTestProject1.States;

namespace UnitTestProject1
{
	public class IntroScene : Screen
	{
		public IntroScene(Game game) : base(game)
		{
		}

		public override void Activated()
		{
			base.Activated();
		}

		public override void Deactivated()
		{
			base.Deactivated();
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}

		public override void Draw(GameTime gameTime, Action<IDrawable>[] actions = null)
		{
			base.Draw(gameTime, actions);
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			Components.OfType<IGameStateSensitive>();

			base.Update(gameTime);
		}

		protected override void LoadContent()
		{
			base.LoadContent();
		}
	}
}
