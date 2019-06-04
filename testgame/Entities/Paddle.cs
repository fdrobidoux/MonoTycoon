using System;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using testgame.Core;
using testgame.Mechanics;

namespace testgame.Entities
{
    public class Paddle : DrawableGameComponent
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Size = new Vector2(50, 100);
        public Rectangle Bounds => new Rectangle(Position.ToPoint(), Size.ToPoint());

        public Texture2D PaddleTexture;
        public Team Team;

        public Paddle(Game game, Team team) : base(game)
        {
            Team = team;
        }

        public override void Initialize()
        {
            base.Initialize(); // Will call `LoadContent()`;

            IMatch match = Game.Services.GetService<IMatch>();
            match.MatchStateChanges += OnMatchStateChanges;
            IRound round = match.CurrentRound;
            if (round != null)
                round.RoundStateChanges += OnRoundStateChanges;

            float x;
            switch (Team)
            {
                case Team.Blue:
                    x = GraphicsDevice.Viewport.Bounds.Left;
                    break;
                case Team.Red:
                    x = GraphicsDevice.Viewport.Bounds.Right - Size.X;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Team), Team, null);
            }

            Position = new Vector2(x, (GraphicsDevice.Viewport.Height / 2f) - (Size.Y / 2f));

            Visible = true;
            Enabled = true;
        }

        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            IMatch match = (IMatch) sender;

            //_moving = (e.Modified.Any(MatchState.InProgress, MatchState.DemoMode));

            if (e.Modified == MatchState.InstanciatedRound)
            {
                match.CurrentRound.RoundStateChanges += OnRoundStateChanges;
            }
        }

        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (e.Modified == RoundState.NotStarted)
            {
                Enabled = false;
                Visible = false;
            }
            else if (e.Modified == RoundState.WaitingForBallServe)
            {
                Visible = true;
                Enabled = true;
            }
        }

        protected override void LoadContent()
        {
            PaddleTexture = Game.Content.Load<Texture2D>("ship");
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
            Position = new Vector2(Position.X, Mouse.GetState().Y - (Size.Y / 2f));
        }

        private void MoveAI(GameTime gameTime)
        {
            // TODO: Rewrite this.
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GetSpriteBatch().Draw(PaddleTexture, Bounds, Color.White);
        }

        public void ConstrainWithinBounds(Rectangle viewBounds)
        {
            //Vector2 boundOpposite = (Size + Position);

            Position = new Vector2(
                Math.Clamp(Position.X, viewBounds.Left, viewBounds.Right - Size.X),
                Math.Clamp(Position.Y, viewBounds.Top, viewBounds.Bottom - Size.Y)
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