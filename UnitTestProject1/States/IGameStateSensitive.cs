using Microsoft.Xna.Framework;
using MonoTycoon.Core.States;
using System;

namespace UnitTestProject1.States
{
	public interface IGameStateSensitive : IStateSensitive<IGameStateHandler, GameState>
	{ }
}
