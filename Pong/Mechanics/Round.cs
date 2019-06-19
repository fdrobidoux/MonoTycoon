using System;
using System.Diagnostics.SymbolStore;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Core;

namespace Pong.Mechanics
{
    public class Round : GameComponent, IRound
    {
		Match _assignedMatch;

		RoundState _state;
		public RoundState State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                RoundState old = _state;
                _state = value;
                RoundStateChanges?.Invoke(this, old);
            }
        }
        public ushort Number { get; set; }
        public Team ServingTeam { get; set; }

        public event EventHandler<RoundState> RoundStateChanges;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="game">Game instance</param>
		/// <param name="state">Starting state</param>
		/// <param name="number">Number of round we're at.</param>
		/// <param name="servingTeam">Who serves.</param>
		public Round(Game game, RoundState state, ushort number) : base(game)
        {
            State = state;
            Number = number;
        }

        public override void Initialize()
        {
			RoundStateChanges = null;
			RoundStateChanges += StateChanged;
        }

		public new void Dispose()
        {
            RoundStateChanges = null;
            base.Dispose();
		}

		void StateChanged(object sender, RoundState previous)
		{

		}

		internal void AssignMatch(Match match)
		{
			_assignedMatch = match;
		}

		internal void UnassignMatch()
		{
			_assignedMatch = null;
		}
	}

    public interface IRound : IGameComponent, IUpdateable, IDisposable
    {
        RoundState State { get; set; }
        ushort Number { get; }
        Team ServingTeam { get; set; }
        event EventHandler<RoundState> RoundStateChanges;
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
                case RoundState.Completed:  return Color.GreenYellow;
                case RoundState.InProgress: return Color.GhostWhite;
                case RoundState.NotStarted: return Color.IndianRed;
                case RoundState.WaitingForBallServe: return Color.LightGoldenrodYellow;
                default: return Color.DarkGray;
            }
        }
    }
}