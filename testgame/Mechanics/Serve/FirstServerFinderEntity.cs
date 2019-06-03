using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Core;
using MonoTycoon.Core.Common;
using MonoTycoon.Core.Graphics;
using MonoTycoon.Core.Physics;
using System;
using System.Collections.Generic;
using testgame.Entities;

namespace testgame.Mechanics
{
    public class FirstServerFinderEntity : DrawableGameComponent
    {
        private Ball TheBall;

        private Point BallPosition;

        private Team currentTeam;

        private bool isChoosing = true;

        public TimerTask timerSwitcharooDo;
        public float multiplicateur = 1.1F;

        public TimerTask timerEndSwitcharoo;

        public TimerTask timerEndScaling;
        private double ballScale;

        public FirstServerFinderEntity(Game game, Ball ball) : base(game)
        {
            TheBall = ball;
            timerSwitcharooDo = new TimerTask(alternateBallPosition, 33, true);
            timerEndSwitcharoo = new TimerTask(onEndSwitcharoo, int.MaxValue, false);
            timerEndScaling = new TimerTask(finalizeFindingFirstServer, TimeSpan.FromSeconds(2).TotalMilliseconds, false);
        }

        public override void Initialize()
        {
            base.Initialize();
            
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

            ballScale = 3F;
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
                ballScale = setBallScale(gt) * gt.ElapsedGameTime.TotalMilliseconds;
            }
        }

        private void alternateBallPosition()
        {
            Vector2 center = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            center.X /= 2f;
            BallPosition = center.ToPoint();

            // This is where we switch teams.
            currentTeam = currentTeam.Opposite();

            // Shortcut to put ball at the opposite side, if it is the opposite site.
            if (currentTeam.GetScreenPosition() == Direction.Right) 
                BallPosition.X *= 3;

            timerSwitcharooDo.IntervalMs *= multiplicateur;
        }

        private void onEndSwitcharoo()
        {
            timerSwitcharooDo.Enabled = false;
            timerEndScaling.Enabled = true;
        }

        private void finalizeFindingFirstServer()
        {
            Match match = (Match) Game.Services.GetService<IMatch>();
            isChoosing = false;

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
            int startingSize = 100;

            if (timerEndScaling.Enabled && !timerEndScaling.IsFinished)
                startingSize = (int)Math.Round(startingSize + ballScale, 0);

            Point scaledBallSize = new Point(startingSize);

            //var origin = BallTexture.Bounds.Center.ToVector2();
            Game.GetSpriteBatch().Draw(TheBall.Sprite, new Rectangle(BallPosition - scaledBallSize.DivideBy(2), scaledBallSize), Color.White);
        }

        private double setBallScale(GameTime gt)
        {
            double x = TimeSpan.FromMilliseconds(timerEndScaling.ElapsedMs).TotalSeconds;
            return Math.Abs(Math.Sin(x * 5));
        }
    }
}
