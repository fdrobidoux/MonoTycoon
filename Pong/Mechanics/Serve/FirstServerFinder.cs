using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon;
using MonoTycoon.Extensions;
using MonoTycoon.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using Pong.Core;
using Pong.Entities;
using MonoTycoon.States;

namespace Pong.Mechanics.Serve
{
    public class FirstServerFinder : DrawableGameComponent, IMatchStateSensitive
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
            timerSwitcharooDo = new TimerTask(AlternateBallPosition, 33, true) { Enabled = false } ;
            timerEndSwitcharoo = new TimerTask(onEndSwitcharoo, int.MaxValue, false) { Enabled = false };
            timerEndScaling = new TimerTask(finalizeFindingFirstServer, 2000, false) { Enabled = false };
        }

        public override void Initialize()
        {
            IMatch _match = Game.Services.GetService<IMatch>();
            _match.StateChanges += StateChanged;

            TheBall.Transform.Scale = 1f;
            Visible = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sfx_Switch = Game.Content.Load<SoundEffect>("sfx/firstserver_switch");
            sfx_Chosen = Game.Content.Load<SoundEffect>("sfx/firstserver_chosen");
        }

        public void StateChanged(IMachineStateComponent<RoundState> round, RoundState previousState)
        {
            if (round.State != RoundState.NotStarted)
            {
                Enabled = false;
                Visible = false;
            }
        }

        public void StateChanged(IMachineStateComponent<MatchState> sender, MatchState previousState)
        {
            if (!(sender is IMatch match)) return;

            if (match.State == MatchState.InstanciatedRound)
            {
                Enabled = true;
                match.CurrentRound.StateChanges += StateChanged;
            }
            else if (match.State == MatchState.FindingFirstServer)
            {
                Visible = true;
                currentTeam = (new Random().Next(2) == 1) ? Team.Blue : Team.Red;
                timerSwitcharooDo.Reset(true);
                timerEndSwitcharoo.IntervalMs = TimeSpan.FromMilliseconds(new Random().Next(2000, 3001)).TotalMilliseconds;
                timerEndSwitcharoo.Reset(true);
                timerEndScaling.Reset(false);
            }
            else if (previousState == MatchState.FindingFirstServer)
            {
                match.CurrentRound.StateChanges -= StateChanged;
            }
        }

        /// <summary>
        /// ROUND EVENTS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="previous"></param>
        private void OnRoundStateChanges(object sender, RoundState e)
        {
            if (!(sender is IRound round)) return;

            if (round.State != RoundState.NotStarted)
            {
                Enabled = false;
                Visible = false;
            }
        }

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
            IMatch match = Game.Services.GetService<IMatch>();

            // Remove visibility.
            Visible = false;
            Enabled = false;

            timerEndScaling.Enabled = false;

            // Set the team that'll be serving.
            match.CurrentRound.ServingTeam = currentTeam;

            // Set match state.
            match.State = MatchState.InProgress;

            // Set the round's state.
            match.CurrentRound.State = RoundState.WaitingForBallServe;
        }

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