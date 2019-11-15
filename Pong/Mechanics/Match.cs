using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.States;

namespace Pong.Mechanics
{
    public class Match : MachineStateComponent<MatchState>, IMatch
    {
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

        public Match(Game game) : base(game)
        {
            
        }

        #region "Implementation of `GameComponent`"

        public override void Initialize()
        {
            base.Initialize();
            _scoreBlue = 0;
            _scoreRed = 0;
		}

        #endregion

        #region "Implementation of `IRoundStateSensitive`"

        public void StateChanged(IMachineStateComponent<RoundState> round, RoundState previousState)
        {
            
        }

        #endregion

        #region "Implementation of `MachineStateComponent<MatchState>`"

        public override Type GetSensitivityType() => typeof(IMatchStateSensitive);

        protected override void WhenOwnStateChanges(MatchState previous)
        {
            base.WhenOwnStateChanges(previous);
            // TODO: Code this.
        }

        #endregion
        
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
            if (CurrentRound == null)
            {
                CurrentRound = new Round(Game, RoundState.NotStarted, 1);
                CurrentRound.Initialize();
				Game.Components.Add(CurrentRound);
                this.State = MatchState.InstanciatedRound;
            }
            else
            {
                CurrentRound.Number++;
                CurrentRound.State = RoundState.WaitingForBallServe;
            }

            return CurrentRound;
        }
    }

    public interface IMatch : IMachineStateComponent<MatchState>, IRoundStateSensitive
    {
        event EventHandler<Team> TeamScores;
        int ScoreBlue { get; }
        int ScoreRed { get; }
        IRound CurrentRound { get; }
        void AddOnePointTo(Team team);
        int GetScore(Team team);
        IRound StartNewRound();
    }

    public enum MatchState : int
	{
		/// <summary>
		/// When `_currentRound` now has an instance of a `Round` object. Finding the first server...
		/// </summary>
		InstanciatedRound = -1,
		/// <summary>
		/// Nothing is being done right now.
		/// </summary>
		NotStarted = 0,
		/// <summary>
		/// First server is being found.
		/// </summary>
		FindingFirstServer = 1,
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

		/// <summary>
		/// Returns true if the state is a phantom state (i.e. won't trigger events).
		/// </summary>
		public static bool IsPhantomState(this MatchState thisState) 
			=> thisState < 0;
	}
}