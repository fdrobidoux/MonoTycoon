using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Core;
using MonoTycoon.Core.Common;
using MonoTycoon.Core.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using testgame.Core;
using testgame.Entities;

namespace testgame.Mechanics.Serve
{
    public class FirstServerFinder : DrawableGameComponent
    {
        private Ball TheBall;
        private Team currentTeam;

        # region "Timers"
        public TimerTask timerSwitcharooDo;
        public double multiplicateurInterval = 1.1D;
        public TimerTask timerEndSwitcharoo;
        public TimerTask timerEndScaling;
        #endregion

        SoundEffect soundSwitchingBall;
        
        public FirstServerFinder(Game game, Ball ball) : base(game)
        {
            TheBall = ball;
            timerSwitcharooDo = new TimerTask(alternateBallPosition, 33, true);
            timerEndSwitcharoo = new TimerTask(onEndSwitcharoo, int.MaxValue, false);
            timerEndScaling = new TimerTask(finalizeFindingFirstServer, TimeSpan.FromSeconds(2).TotalMilliseconds, false);

            EnabledChanged += OnEnabledChanged;
        }

        public override void Initialize()
        {
            base.Initialize();

            IMatch _match = Game.Services.GetService<IMatch>();
            _match.MatchStateChanges += onMatchStateChanges;
            
            currentTeam = (new Random().Next(2) == 1) ? Team.Blue : Team.Red;

            timerSwitcharooDo.Reset();
            timerSwitcharooDo.Enabled = true;
            timerSwitcharooDo.Recurring = true;

            timerEndSwitcharoo.Reset();
            timerEndSwitcharoo.Enabled = true;
            timerEndSwitcharoo.IntervalMs = TimeSpan.FromMilliseconds(new Random().Next(2000, 3001)).TotalMilliseconds;

            timerEndScaling.Reset();
            timerEndScaling.Enabled = false;

            TheBall.Transform.Scale = 1f;

            Enabled = false;
            Visible = false;
        }

        protected new void OnEnabledChanged(object sender, EventArgs args)
        {
            IMatch _match = Game.Services.GetService<IMatch>();
            match.CurrentRound.RoundStateChanges -= onRoundStateChanges;
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
            else if (e.Old == MatchState.InstanciatedRound)
            {
                match.CurrentRound.RoundStateChanges -= onRoundStateChanges;
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
            
            
        }

        private void alternateBallPosition()
        {
            Vector2 newPosition = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            newPosition.X /= 2f;

            // Shortcut to put ball at the opposite side, if it is the opposite site.
            if ((currentTeam = currentTeam.Opposite()).GetScreenPosition() == Direction.Right)
                newPosition.X *= 3f;

            TheBall.Transform.Location = newPosition;

            calculateRectangle();

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

            TheBall.Visible = true;
            TheBall.Transform.Scale = 1f;

            // Remove visibility.
            Visible = false;

            timerEndScaling.Enabled = false;

            // Set the team that'll be serving.
            match.CurrentRound.ServingTeam = currentTeam;

            // Set match state.
            match.State = MatchState.InProgress;

            // Set the round's state.
            match.CurrentRound.State = RoundState.WaitingForBallServe;
        }

        private void calculateRectangle()
        {
            //rect = TheBall.Transform.ToRectangle();

            //if (timerEndScaling.Enabled && !timerEndScaling.IsFinished)
            //    rect.Size = rect.Size.Scale(ballScale);

            //rect = new Rectangle(rect.Location - rect.Size.DivideBy(2), rect.Size);

            //return rect;
        }

        /// <summary>
        /// Draw method.
        /// </summary>
        /// <param name="gt"></param>
        public override void Draw(GameTime gt)
        {
            if (timerEndScaling.Enabled)
                TheBall.Transform.Scale = getBallScale();
        }

        private float getBallScale()
        {
            float x = (float) TimeSpan.FromMilliseconds(timerEndScaling.ElapsedMs).TotalSeconds;
            return MathF.Abs(MathF.Sin((6 * x) + 0.5f)) + 0.5f;
        }
    }
}
