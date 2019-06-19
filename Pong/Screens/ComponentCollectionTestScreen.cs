using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoTycoon.Screens;
using MonoTycoon.States;
using Pong.Mechanics;

namespace Pong.Screens
{
	public class ComponentCollectionTestScreen : Screen
	{
		List<IStateSensitive<IGameComponent, Enum>> listMatchStateSensitive;

		public ComponentCollectionTestScreen(Game game) : base(game)
		{
			
		}


	}
}
