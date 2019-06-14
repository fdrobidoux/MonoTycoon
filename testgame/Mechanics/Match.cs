using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Core;

namespace Pong.Mechanics
{
    public class Match : GameComponent, IMatch
    {
        MatchState _state;
        public MatchState State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                MatchState previous = _state;
                _state = value;
				_hasChanged = true;
				MatchStateChanges?.Invoke(this, previous);
            }
        }

		bool _hasChanged;

        int _scoreBlue;
        public int ScoreBlue
        {
            get => _scoreBlue;
            private set
            {
                if (value == _scoreBlue)
                    return;
                bool valueIncreased = value > _scoreBlue;
                _scoreBlue = value;
                if (valueIncreased)
					TeamScores?.Invoke(this, Team.Blue);
            }
        }

        int _scoreRed;
        public int ScoreRed
        {
            get => _scoreRed;
            private set
            {
                if (value == _scoreRed) return;
                bool valueIncreased = value > _scoreRed;
                _scoreRed = value;
                if (valueIncreased)
					TeamScores?.Invoke(this, Team.Red);
            }
        }

		Round _currentRound;
        public Round CurrentRound 
		{
			get => _currentRound; 
			set 
			{
				_currentRound?.UnassignMatch();
				_currentRound = value;
				_currentRound.AssignMatch(this);
			}
		}
		IRound IMatch.CurrentRound => CurrentRound;

        public event EventHandler<Team> TeamScores;
        public event EventHandler<MatchState> MatchStateChanges;

        public Match(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
			_hasChanged = false;
            _scoreBlue = 0;
            _scoreRed = 0;
			MatchStateChanges = null;
			MatchStateChanges += OnMatchStateChanges;
		}

		public override void Update(GameTime gameTime)
		{
			resetStateChangedFlag();
		}

		private void resetStateChangedFlag()
		{
			_hasChanged = false;
		}

		#region "Unique To `Match`"

		public void AddOnePointTo(Team team)
        {
            switch (team)
            {
                case Team.Blue: ScoreBlue++; return;
                case Team.Red: ScoreRed++; return;
                default: throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }

        public int GetScore(Team team)
        {
            switch (team)
            {
                case Team.Red: return ScoreRed;
                case Team.Blue: return ScoreBlue;
                default: throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }

        public IRound StartNewRound()
        {
            if (CurrentRound == null || State == MatchState.Finished)
            {
                CurrentRound = new Round(Game, RoundState.NotStarted, 1);
                CurrentRound.Initialize();
                State = MatchState.InstanciatedRound;
            }
            else
            {
                CurrentRound.Number++;
                CurrentRound.State = RoundState.WaitingForBallServe;
            }

            return CurrentRound;
        }

        void OnMatchStateChanges(object sender, MatchState previous)
        {
            
        }

        #endregion
    }

    public interface IMatch : IGameComponent
    {
        MatchState State { get; set; }
        int ScoreBlue { get; }
        int ScoreRed { get; }
        IRound CurrentRound { get; }
        
        event EventHandler<Team> TeamScores;
        event EventHandler<MatchState> MatchStateChanges;

        void AddOnePointTo(Team team);
        int GetScore(Team team);
        IRound StartNewRound();
    }

    public enum MatchState : byte
    {
        /// <summary>
        /// Nothing is being done right now.
        /// </summary>
        NotStarted,
        /// <summary>
        /// When `_currentRound` now has an instance of a `Round` object. Finding the first server...
        /// </summary>
        InstanciatedRound,
		/// <summary>
		/// First server is being found.
		/// </summary>
		FindingFirstServer,
        /// <summary>
        /// Player can control; Refer to RoundState from then on.
        /// </summary>
        InProgress,
        /// <summary>
        /// Winner text is shown, will go back to title screen with `DemoMode`
        /// </summary>
        Finished,
        /// <summary>
        /// None of the paddles ever fail to hit the ball; always is in progress.
        /// </summary>
        DemoMode
    }

    public static class MatchStateExtensions
    {
        /// <summary>
        /// Returns true if any of the states given equal the instance MatchState.
        /// </summary>
        public static bool Any(this MatchState thisMatchState, params MatchState[] statesToMatch) 
            => statesToMatch.Any(state => thisMatchState == state);

        /// <summary>
        /// Returns true if none of the states given are different from the instance MatchState.
        /// </summary>
        public static bool None(this MatchState thisMatchState, params MatchState[] statesToAvoid) 
            => statesToAvoid.All(state => thisMatchState != state);
    }
}