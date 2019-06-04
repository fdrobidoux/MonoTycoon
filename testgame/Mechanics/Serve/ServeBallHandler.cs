using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoTycoon.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using testgame.Core;
using testgame.Entities;

namespace testgame.Mechanics.Serve
{
    public class ServeBallHandler : GameComponent
    {
        private const int SECONDS_TIL_CAN_SERVE = 3;

        private bool canServe;

        public Paddle ServingPaddle { get; private set; }
        public Ball TheBall { get; private set; }

        #region "Timers"
        private TimerTask timer_AllowServing;
        #endregion

        public ServeBallHandler(Game game) : base(game)
        {
            timer_AllowServing = new TimerTask(allowServing, TimeSpan.FromSeconds(SECONDS_TIL_CAN_SERVE).TotalMilliseconds, false);
        }

        public override void Initialize()
        {
            Enabled = false;

            IMatch match = Game.Services.GetService<IMatch>();
            match.MatchStateChanges += OnMatchStateChanges;

            timer_AllowServing.Reset();
            timer_AllowServing.Enabled = true;

            canServe = false;
        }

        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            if (!(sender is IMatch match))
                return;

            if (e.IsNow(MatchState.InstanciatedRound))
                match.CurrentRound.RoundStateChanges += OnRoundStateChanges;
        }

        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            Enabled = e.IsNow(RoundState.WaitingForBallServe);
        }

        public override void Update(GameTime gt)
        {
            timer_AllowServing.Update(gt);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Enabled = false;
                var blah = new Vector2(-1f, 1f);
                if (ServingPaddle.Team == Team.Blue)
                    blah *= -1;

                // Set velocity to Ball.
                TheBall.Direction = blah;

                // Set round state to `InProgress`.
                Game.Services.GetService<IMatch>().CurrentRound.State = RoundState.InProgress;
            }
            else
            {
                TheBall.Transform.Location = new Vector2(
                    x: TheBall.Transform.Location.X,
                    y: ServingPaddle.Transform.Location.Y + (ServingPaddle.Transform.Size.Height / 2));
            }
        }

        public void AssignEntitiesNecessaryForServing(Ball ball, Paddle servingPaddle)
        {
            TheBall = ball;
            ServingPaddle = servingPaddle;
        }

        public void UnassignEntitiesNecessaryForServing()
        {
            TheBall = null;
            ServingPaddle = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                UnassignEntitiesNecessaryForServing();
        }

        private void allowServing()
        {
            canServe = true;
        }
    }
}
