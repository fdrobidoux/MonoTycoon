using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using testgame.Core;

namespace testgame.Mechanics
{
    public class Match : GameComponent, IMatch
    {
        private int _scoreBlue;
        private int _scoreRed;
        private MatchState _state;
        private Round _currentRound = null;

        public Match(Game game, MatchState? startingState = null) : base(game)
        {
            _state = startingState ?? MatchState.DemoMode;
        }

        public MatchState State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                MatchState old = _state;
                _state = value;
                MatchStateChanges?.Invoke(this, new ValueChangedEvent<MatchState>(old, _state));
            }
        }
        public int ScoreBlue
        {
            get => _scoreBlue;
            private set
            {
                if (value == _scoreBlue)
                    return;
                bool wasReduced = value < _scoreBlue;
                _scoreBlue = value;
                if (wasReduced) return;
                TeamScores?.Invoke(this, Team.Blue);
            }
        }
        public int ScoreRed
        {
            get => _scoreRed;
            private set
            {
                if (value == _scoreRed) return;
                int old = _scoreRed;
                _scoreRed = value;
                if (value < _scoreRed) return;
                TeamScores?.Invoke(this, Team.Red);
            }
        }

        public Round CurrentRound { get => _currentRound; set => _currentRound = value; }
        IRound IMatch.CurrentRound => _currentRound;

        public event EventHandler<Team> TeamScores;
        public event EventHandler<ValueChangedEvent<MatchState>> MatchStateChanges;

        public override void Initialize()
        {
            MatchStateChanges = null;
            MatchStateChanges += OnMatchStateChanges;
            _scoreBlue = 0;
            _scoreRed = 0;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            CurrentRound?.Update(gameTime);
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

        public Round StartNewRound()
        {
            if (_currentRound == null)
            {
                CurrentRound = new Round(Game, RoundState.NotStarted, 1, Team.Blue);
                State = MatchState.InstanciatedRound;
            }
            else
            {
                CurrentRound.Number++;
                CurrentRound.ServingTeam = Team.Blue; // TODO: Set serving team based on last round winner.
                CurrentRound.State = RoundState.NotStarted;
            }

            return _currentRound;
        }

        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            /*
            if (!(sender is Match match)) 
                return;
            
            if (e.Modified == MatchState.InstanciatedRound)
                // TODO: Change it so that InProgress is set only after a timer.
                State = MatchState.InProgress;
            */
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
        event EventHandler<ValueChangedEvent<MatchState>> MatchStateChanges;

        void AddOnePointTo(Team team);
        Round StartNewRound();
    }

    public enum MatchState : byte
    {
        /// <summary>
        /// Nothing is being done right now.
        /// </summary>
        NotStarted,
        /// <summary>
        /// When `_currentRound` now has an instance of a `Round` object.
        /// </summary>
        InstanciatedRound,
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

        //public static bool All(this MatchState thisMatchState, params MatchState[] statesThatAllNeedToMatch) 
        //    => statesThatAllNeedToMatch.All(state => thisMatchState == state);
    }
}