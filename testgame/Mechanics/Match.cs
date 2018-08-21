using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using testgame.Core;

namespace testgame.Mechanics
{
    public interface IMatch
    {
        MatchState State { get; set; }
        int ScoreBlue { get; }
        int ScoreRed { get; }
        IRound CurrentRound { get; }
        event EventHandler<Team> TeamScores;
        event EventHandler<ValueChangedEvent<MatchState>> MatchStateChanges;
    }
    
    public class Match : DrawableGameComponent, IMatch, IActuallyDrawable
    {
        private int _scoreBlue;
        private int _scoreRed;
        private MatchState _state;
        private Round _currentRound = null;
        
        public Match(Game game) : base(game)
        {
            _state = MatchState.DemoMode;
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
                if (value == _scoreBlue) return;
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

        IRound IMatch.CurrentRound => _currentRound;
        
        public Round CurrentRound
        {
            get => _currentRound;
            set => _currentRound = value;
        }

        public event EventHandler<Team> TeamScores;
        public event EventHandler<ValueChangedEvent<MatchState>> MatchStateChanges;

        public void Initialize()
        {
            MatchStateChanges += OnMatchStateChanges;
            CurrentRound = null;
            _scoreBlue = 0;
            _scoreRed = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void AddOnePointTo(Team team)
        {
            switch (team)
            {
                case Team.Blue:
                    ScoreBlue++;
                    break;
                case Team.Red:
                    ScoreRed++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }

        public Round StartNewRound()
        {
            if (_currentRound == null)
            {
                CurrentRound = new Round {State = RoundState.NotStarted, ServingTeam = Team.Blue};
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
            if (sender is Match match)
            {
                if (e.Modified == MatchState.InstanciatedRound)
                {
                    State = MatchState.InProgress;
                }
            }
        }
    }
    
    public enum MatchState
    {
        /// <summary>
        /// Nothing is being done right now.
        /// </summary>
        NotStarted,
        /// <summary>
        /// `_currentRound` now has an instance of a `Round` object.
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
        /// Returns true if any state given equals the instance MatchState.
        /// </summary>
        /// <param name="thisMatchState"></param>
        /// <param name="statesToMatch"></param>
        /// <returns></returns>
        public static bool Any(this MatchState thisMatchState, params MatchState[] statesToMatch)
        {
            foreach (MatchState state in statesToMatch)
            {
                if (thisMatchState == state)
                    return true;
            }

            return false;
        }
    }
}