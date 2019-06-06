using System;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTycoon.Core;
using MonoTycoon.Core.Physics;
using Pong.Core;
using Pong.Mechanics;

namespace Pong.Entities
{
    public class Paddle : DrawableGameComponent
    {
        public Transform2 Transform { get; set; }

        public Texture2D PaddleTexture;
        public Team Team;

        public Paddle(Game game, Team team) : base(game)
        {
            Team = team;
            Transform = Transform2.Zero;
        }

        public override void Initialize()
        {
            base.Initialize(); // Will call `LoadContent()`;

            IMatch match = Game.Services.GetService<IMatch>();
            match.MatchStateChanges += OnMatchStateChanges;

            IRound round = match.CurrentRound;
            if (round != null)
                round.RoundStateChanges += OnRoundStateChanges;

            Transform.Size = new Size2(50, 100);

            float x;
            switch (Team)
            {
                case Team.Blue:
                    x = GraphicsDevice.Viewport.Bounds.Left;
                    break;
                case Team.Red:
                    x = GraphicsDevice.Viewport.Bounds.Right - Transform.Size.Width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Team), Team, null);
            }
            
            Transform.Location = new Vector2(x, (GraphicsDevice.Viewport.Height) - (Transform.Size.Height) / 2f);

            Visible = true;
            Enabled = true;
        }

        private void OnMatchStateChanges(object sender, MatchState previous)
        {
            if (!(sender is IMatch match))
                return;

            //_moving = (e.Modified.Any(MatchState.InProgress, MatchState.DemoMode));

            if (match.State  == MatchState.InstanciatedRound)
            {
                match.CurrentRound.RoundStateChanges += OnRoundStateChanges;
            }
        }

        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (e.Current == RoundState.NotStarted)
            {
                Enabled = false;
                Visible = false;
            }
            else if (e.Current == RoundState.WaitingForBallServe)
            {
                Visible = true;
                Enabled = true;
            }
        }

        protected override void LoadContent()
        {
            // TODO: Change sprite for paddle.
            PaddleTexture = Game.Content.Load<Texture2D>("textures/ship");
        }

        public override void Update(GameTime gameTime)
        {
            IMatch match = Game.Services.GetService<IMatch>();

            if (Team == Team.Red || match.State == MatchState.DemoMode)
                MoveAI(gameTime);
            else if (Team == Team.Blue)
                MoveWithMouse(gameTime);
            else
                throw new NotImplementedException();

            ConstrainWithinBounds(GraphicsDevice.Viewport.Bounds);
        }

        private void MoveWithMouse(GameTime gameTime)
        {
            Transform.Location = new Vector2(
                x: Transform.Location.X,
                y: Mouse.GetState().Y - (Transform.Size.Height / 2)
            );
        }

        private void MoveAI(GameTime gameTime)
        {
            // TODO: Rewrite this.
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GetSpriteBatch().Draw(PaddleTexture, Transform.ToRectangle(), Color.White);
        }

        public void ConstrainWithinBounds(Rectangle viewBounds)
        {
            //Vector2 boundOpposite = (Size + Position);

            Transform.Location = new Vector2(
                Math.Clamp(Transform.Location.X, viewBounds.Left, viewBounds.Right - Transform.Size.Width),
                Math.Clamp(Transform.Location.Y, viewBounds.Top, viewBounds.Bottom - Transform.Size.Height)
            );

            //if (Position.X < viewBounds.Left)
            //    Position.X = viewBounds.Left;

            //if (boundOpposite.X > viewBounds.Right)
            //    Position.X = viewBounds.Right - Size.X;

            //if (Position.Y < viewBounds.Top)
            //    Position.Y = viewBounds.Top;

            //if (boundOpposite.Y > viewBounds.Bottom)
            //    Position.Y = viewBounds.Bottom - Size.Y;
        }
    }
}