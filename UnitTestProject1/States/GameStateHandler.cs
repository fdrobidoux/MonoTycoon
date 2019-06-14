using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoTycoon.Core.States;

namespace UnitTestProject1.States
{
	public class GameStateHandler : MachineStateComponent<GameState>, IGameStateHandler
	{
		public override Type GetSensitivityType() => typeof(IGameStateSensitive);

		public GameStateHandler(Game game) : base(game)
		{
		}

		public override void Initialize()
		{
			base.Initialize();

		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		protected override void WhenOwnStateChanges(GameState previous)
		{
			base.WhenOwnStateChanges(previous);
		}
	}

	public enum GameState : ushort
	{
		INTRO,
		PLAYING_STAGE,
		PAUSED,
		UNPAUSED
	}

	public interface IGameStateHandler : IMachineStateComponent<GameState>
	{

	}
}
