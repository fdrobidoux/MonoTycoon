using System;
using System.Diagnostics.SymbolStore;
using testgame.Core;

namespace testgame.Mechanics
{
    public interface IRound
    {
        RoundState State { get; set; }
        ushort Number { get; }
        Team ServingTeam { get; }
        event EventHandler<ValueChangedEvent<RoundState>> RoundStateChanges;
    }

    public class Round : IRound, IDisposable
    {
        private RoundState _state;

        public RoundState State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                RoundState old = _state;
                _state = value;
                RoundStateChanges?.Invoke(this, new ValueChangedEvent<RoundState>(old, value));
            }
        }

        public ushort Number { get; set; }

        public Team ServingTeam { get; set; }

        public event EventHandler<ValueChangedEvent<RoundState>> RoundStateChanges;

        public void Dispose()
        {
            RoundStateChanges = null;
        }
    }

    public enum RoundState
    {
        /// <summary>
        /// Most likely doing an intro.
        /// </summary>
        NotStarted,
        /// <summary>
        /// Ball needs to be served by whichever player shall get the ball.
        /// </summary>
        WaitingForBallServe,
        /// <summary>
        /// Ping-Pong happening.
        /// </summary>
        InProgress,
        /// <summary>
        /// Winner was found. Will soon change round.
        /// </summary>
        Completed,
    }

    public static class RoundStateExtensions
    {
        /// <summary>
        /// Returns true if any state given equals the instance MatchState.
        /// </summary>
        /// <param name="thisRoundState"></param>
        /// <param name="statesToMatch"></param>
        /// <returns></returns>
        public static bool Any(this RoundState thisRoundState, params RoundState[] statesToMatch)
        {
            foreach (RoundState state in statesToMatch)
            {
                if (thisRoundState == state)
                    return true;
            }

            return false;
        }

        public static bool None(this RoundState thisRoundState, params RoundState[] statesToAvoid)
        {
            foreach (RoundState state in statesToAvoid)
            {
                if (thisRoundState == state)
                    return false;
            }

            return true;
        }
        
        /*
        public static bool All(this RoundState thisRoundState, params RoundState[] statesThatAllNeedToMatch)
        {
            foreach (RoundState state in statesThatAllNeedToMatch)
            {
                if (thisRoundState != state)
                    return false;
            }

            return true;
        }
        */
    }
}