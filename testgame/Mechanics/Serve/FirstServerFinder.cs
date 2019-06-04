using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Core;
using MonoTycoon.Core.Common;
using MonoTycoon.Core.Graphics;
using MonoTycoon.Core.Physics;
using System;
using testgame.Core;
using testgame.Entities;

namespace testgame.Mechanics.Serve
{
    public class FirstServerFinder : DrawableGameComponent
    {
        private Ball TheBall;
        private Team currentTeam;
        private bool isChoosing = true;

        private Vector2 ballPosition;
        private double ballScale;
        private Rectangle lastRectangle;

        # region "Timers"
        public TimerTask timerSwitcharooDo;
        public double multiplicateurInterval = 1.1D;
        public TimerTask timerEndSwitcharoo;
        public TimerTask timerEndScaling;
        #endregion

        public FirstServerFinder(Game game, Ball ball) : base(game)
        {
            TheBall = ball;
            timerSwitcharooDo = new TimerTask(alternateBallPosition, 33, true);
            timerEndSwitcharoo = new TimerTask(onEndSwitcharoo, int.MaxValue, false);
            timerEndScaling = new TimerTask(finalizeFindingFirstServer, TimeSpan.FromSeconds(2).TotalMilliseconds, false);
        }

        public override void Initialize()
        {
            base.Initialize();

            IMatch _match = Game.Services.GetService<IMatch>();
            _match.MatchStateChanges += onMatchStateChanges;

            isChoosing = true;
            currentTeam = (new Random().Next(2) == 1) ? Team.Blue : Team.Red;

            timerSwitcharooDo.Reset();
            timerSwitcharooDo.Enabled = true;
            timerSwitcharooDo.Recurring = true;

            timerEndSwitcharoo.Reset();
            timerEndSwitcharoo.Enabled = true;
            timerEndSwitcharoo.IntervalMs = TimeSpan.FromMilliseconds(new Random().Next(2000, 3001)).TotalMilliseconds;

            timerEndScaling.Reset();
            timerEndScaling.Enabled = false;

            ballScale = 1D;

            Enabled = false;
            Visible = false;
        }

        /// <summary>
        /// MATCH EVENTS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            if (!(sender is IMatch match))
                return;

            if (e.Modified == MatchState.InstanciatedRound)
            {
                match.CurrentRound.RoundStateChanges += onRoundStateChanges;
                Enabled = true;
                Visible = true;
            }
        }

        /// <summary>
        /// ROUND EVENTS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (e.Modified != RoundState.NotStarted)
            {
                Enabled = false;
                Visible = false;
            }
        }

        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="gt"></param>
        public override void Update(GameTime gt)
        {
            timerSwitcharooDo.Update(gt);
            timerEndSwitcharoo.Update(gt);
            timerEndScaling.Update(gt);

            if (timerEndScaling.Enabled)
            {
                ballScale = getBallScale(gt) * gt.ElapsedGameTime.TotalMilliseconds;
                lastRectangle = calculateRectangle();
            }
        }

        private void alternateBallPosition()
        {
            Vector2 newPosition = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            newPosition.X /= 2f;

            // Shortcut to put ball at the opposite side, if it is the opposite site.
            if ((currentTeam = currentTeam.Opposite()).GetScreenPosition() == Direction.Right)
                newPosition.X *= 3f;

            ballPosition = newPosition;

            lastRectangle = calculateRectangle();

            timerSwitcharooDo.IntervalMs *= multiplicateurInterval;
        }

        private void onEndSwitcharoo()
        {
            timerSwitcharooDo.Enabled = false;
            timerEndScaling.Enabled = true;
        }

        private void finalizeFindingFirstServer()
        {
            Match match = (Match)Game.Services.GetService<IMatch>();

            isChoosing = false;

            TheBall.Visible = true;
            Visible = false;

            TheBall.Transform.Location = ballPosition;

            // Set the team that'll be serving.
            match.CurrentRound.ServingTeam = currentTeam;

            // Set match state.
            match.State = MatchState.InProgress;

            // Set the round's state.
            match.CurrentRound.State = RoundState.WaitingForBallServe;
        }

        private Rectangle calculateRectangle()
        {
            int startingSize = TheBall.Transform.Size.Width;

            if (timerEndScaling.Enabled && !timerEndScaling.IsFinished)
                startingSize = (int)Math.Round(startingSize + ballScale, 0);

            Vector2 scaledBallSize = new Vector2(startingSize);

            return new Rectangle(
                location: (ballPosition - (scaledBallSize / 2f)).ToPoint(),
                size:     scaledBallSize.ToPoint());
        }

        /// <summary>
        /// Draw method.
        /// </summary>
        /// <param name="gt"></param>
        public override void Draw(GameTime gt)
        {
            Game.GetSpriteBatch().Draw(TheBall.Sprite, lastRectangle, Color.White);
        }

        private double getBallScale(GameTime gt)
        {
            double x = TimeSpan.FromMilliseconds(timerEndScaling.ElapsedMs).TotalSeconds;
            return Math.Abs(Math.Sin((5 * x) + 0.5f)) + 0.5f;
        }
    }
}
