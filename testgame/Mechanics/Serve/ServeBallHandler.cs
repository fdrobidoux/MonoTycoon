using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using testgame.Core;

namespace testgame.Mechanics.Serve
{
    public class ServeBallHandler : GameComponent
    {
        public ServeBallHandler(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            IMatch match = Game.Services.GetService<IMatch>();
            match.MatchStateChanges += OnMatchStateChanges;
        }

        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            if (sender is IMatch match)
            {
                if (e.Modified.Equals(MatchState.InstanciatedRound))
                {
                    match.CurrentRound.RoundStateChanges += OnRoundStateChanges;
                }
            }
        }

        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (e.Modified.Equals(RoundState.WaitingForBallServe))
            {
                Enabled = true;
            }
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
