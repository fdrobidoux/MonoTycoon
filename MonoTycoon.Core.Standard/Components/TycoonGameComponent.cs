using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Components
{
	public class TycoonGameComponent : GameComponent
	{
		public TycoonGameComponent(Game game) : base(game)
		{

		}

		protected override void OnEnabledChanged(object sender, EventArgs args)
		{
			base.OnEnabledChanged(sender, args);
		}

		public override void Initialize()
		{
			base.Initialize();
		}
	}
}
