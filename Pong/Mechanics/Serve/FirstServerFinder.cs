using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon;
using MonoTycoon.Common;
using MonoTycoon.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using Pong.Core;
using Pong.Entities;

namespace Pong.Mechanics.Serve
{
    public class FirstServerFinder : DrawableGameComponent
    {
        private Ball TheBall;
        private Team currentTeam;

        #region "Timers"

        public TimerTask timerSwitcharooDo;
        public double multiplicateurInterval = 1.1D;
        public TimerTask timerEndSwitcharoo;
        public TimerTask timerEndScaling;

        #endregion

        #region "Sounds"

        private SoundEffect sfx_Switch;
        private SoundEffect sfx_Chosen;

        #endregion

        public FirstServerFinder(Game game, Ball ball) : base(game)
        {
            TheBall = ball;
            timerSwitcharooDo = new TimerTask(AlternateBallPosition, 33, true);
            timerEndSwitcharoo = new TimerTask(onEndSwitcharoo, int.MaxValue, false);
            timerEndScaling =
                new TimerTask(finalizeFindingFirstServer, TimeSpan.FromSeconds(2).TotalMilliseconds, false);
        }

        public override void Initialize()
        {
            base.Initialize();

            IMatch _match = Game.Services.GetService<IMatch>();
            _match.MatchStateChanges += OnMatchStateChanges;

            currentTeam = (new Random().Next(2) == 1) ? Team.Blue : Team.Red;

            timerSwitcharooDo.Reset(modEnabled: true);
            timerEndSwitcharoo.Reset(true);
            timerEndSwitcharoo.IntervalMs = TimeSpan.FromMilliseconds(new Random().Next(2000, 3001)).TotalMilliseconds;
            timerEndScaling.Reset(false);

            TheBall.Transform.Scale = 1f;

            Enabled = false;
            Visible = false;
        }

        protected override void LoadContent()
        {
            sfx_Switch = Game.Content.Load<SoundEffect>("sfx/firstserver_switch");
            sfx_Chosen = Game.Content.Load<SoundEffect>("sfx/firstserver_chosen");
        }

        /// <summary>
        /// MATCH EVENTS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="previous"></param>
        private void OnMatchStateChanges(object sender, MatchState previous)
        {
            if (!(sender is IMatch match))
                return;

            if (match.State == MatchState.FindingFirstServer)
            {
                match.CurrentRound.RoundStateChanges += OnRoundStateChanges;
                Enabled = true;
                Visible = true;
            }
            else if (previous == MatchState.FindingFirstServer)
            {
                match.CurrentRound.RoundStateChanges -= OnRoundStateChanges;
            }
        }

        /// <summary>
        /// ROUND EVENTS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="previous"></param>
        private void OnRoundStateChanges(object sender, RoundState e)
        {
            if (!(sender is IRound round))
                return;

            if (round.State != RoundState.NotStarted)
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

        private void AlternateBallPosition()
        {
            Vector2 newPosition = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            newPosition.X /= 2f;

            // Shortcut to put ball at the opposite side, if it is the opposite site.
            if ((currentTeam = currentTeam.Opposite()).GetScreenPosition() == Direction.Right)
                newPosition.X *= 3f;

            TheBall.Transform.Location = newPosition;

            timerSwitcharooDo.IntervalMs *= multiplicateurInterval;

            sfx_Switch.CreateInstance().Play();
        }

        private void onEndSwitcharoo()
        {
            timerSwitcharooDo.Enabled = false;
            timerEndScaling.Enabled = true;

            sfx_Chosen.CreateInstance().Play();
        }

        private void finalizeFindingFirstServer()
        {
            Match match = (Match) Game.Services.GetService<IMatch>();

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

        /// <summary>
        /// Draw method.
        /// </summary>
        /// <param name="gt"></param>
        public override void Draw(GameTime gt)
        {
            if (timerEndScaling.Enabled)
                TheBall.Transform.Scale = DoubleExtensions.ToFloat(_getBallScale());
        }

        private double _getBallScale()
        {
            double x = TimeSpan.FromMilliseconds(timerEndScaling.ElapsedMs).TotalSeconds;
            return Math.Abs(Math.Sin((6 * x) + 0.5)) + 0.5;
        }
    }
}