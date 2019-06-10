using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Mechanics.States;

namespace Pong.Mechanics
{
	public class Round : MachineStateComponent<RoundState>, IRound
	{
		public ushort Number { get; set; }
		public Team ServingTeam { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="game">Game instance</param>
		/// <param name="state">Starting state</param>
		/// <param name="number">Number of round we're at.</param>
		public Round(Game game, RoundState state, ushort number) : base(game)
		{
			State = state;
			Number = number;
		}

		public new event MachineStateEventHandler<IRound, RoundState> StateChanges;

		event MachineStateEventHandler<IMachineStateComponent<RoundState>, RoundState> IMachineStateComponent<RoundState>.StateChanges
		{
			add => StateChanges += value;
			remove => StateChanges -= value;
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		Type IMachineStateComponent<RoundState>.GetSensitivityType() => GetSensitivityType();

		public override Type GetSensitivityType() => typeof(IRoundStateSensitive);
	}

	public interface IRound : IMachineStateComponent<RoundState>, IUpdateable
	{
		ushort Number { get; }
		Team ServingTeam { get; set; }
	}

	public enum RoundState : byte
	{
		/// <summary>Most likely doing an intro.</summary>
		NotStarted,
		/// <summary>Ball needs to be served by whichever player shall get the ball.</summary>
		WaitingForBallServe,
		/// <summary>Ping-Pong happening.</summary>
		InProgress,
		/// <summary>Winner was found. Will soon change round.</summary>
		Completed,
	}

	public static class RoundStateExtensions
	{
		/// <summary>
		/// Returns true if any of the states given equal the instance RoundState.
		/// </summary>
		public static bool Any(this RoundState thisRoundState, params RoundState[] statesToMatch)
			=> statesToMatch.Any(state => thisRoundState == state);

		/// <summary>
		/// Returns true if none of the states given are different from the instance RoundState.
		/// </summary>
		public static bool None(this RoundState thisRoundState, params RoundState[] statesToAvoid)
			=> statesToAvoid.All(state => thisRoundState != state);

		public static Color DebugColor(this RoundState thisRound)
		{
			switch (thisRound)
			{
				case RoundState.Completed: return Color.GreenYellow;
				case RoundState.InProgress: return Color.GhostWhite;
				case RoundState.NotStarted: return Color.IndianRed;
				case RoundState.WaitingForBallServe: return Color.LightGoldenrodYellow;
				default: return Color.DarkGray;
			}
		}
	}
}