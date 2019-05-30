using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
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
        public Texture2D Sprite;
        public Team Team;

        // State booleans.
        private bool _moving = false;

        public Paddle(Game game, Team team) : base(game)
        {
            Team = team;
        }

        public override void Initialize()
        {
            IMatch match = Game.Services.GetService<IMatch>();
            match.TeamScores += OnTeamScores;
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

            base.Initialize(); // Will call `LoadContent()`;
        }

        private void OnTeamScores(object sender, Team e)
        {
            if (sender is IMatch match)
            {
            }
        }

        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            if (sender is IMatch match)
            {
                _moving = (e.Modified == MatchState.NotStarted);

                if (e.Modified == MatchState.InstanciatedRound)
                {
                    match.CurrentRound.RoundStateChanges += OnRoundStateChanges;
                }
                else if (e.Modified == MatchState.Finished)
                {
                    match.CurrentRound.RoundStateChanges -= OnRoundStateChanges;
                }
            }
        }

        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (sender is IRound round)
            {
                if (e.Modified == RoundState.NotStarted)
                {
                    Enabled = false;
                }
                else if (e.Modified == RoundState.InProgress)
                {
                    Enabled = true;
                }
            }
        }

        protected override void LoadContent()
        {
            //Sprite = Game.Content.Load<Texture2D>("ship");
            var strm = new FileStream("C:\\Users\\FelixDev\\source\\projects\\MonoTycoon\\testgame\\Content\\ship.png", FileMode.Open, FileAccess.Read);
            Sprite = Texture2D.FromStream(Game.GraphicsDevice, strm);
            strm.Dispose();
        }

        protected override void UnloadContent()
        {
            Sprite.Dispose();
            Sprite = null;
        }

        public new void Draw(GameTime gameTime)
        {
            Game.GetSpriteBatch().Draw(Sprite, new Rectangle(Position.ToPoint(), Size.ToPoint()), null, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            IMatch match = Game.Services.GetService<IMatch>();

            if (match.State == MatchState.DemoMode)
                MoveAI(gameTime);
            else
            {
                if (match.State.Any(MatchState.InProgress) && match.CurrentRound.State.Any(RoundState.WaitingForBallServe, RoundState.InProgress))
                {
                    if (Team == Team.Blue)
                    {
                        MoveWithMouse(gameTime);
                    }
                    else if (Team == Team.Red)
                    {
                        MoveAI(gameTime);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            ConstrainWithinBounds(GraphicsDevice.Viewport.Bounds);
        }

        private void MoveWithMouse(GameTime gameTime)
        {
            Position.Y = Mouse.GetState().Y;
        }

        private void MoveAI(GameTime gameTime)
        {
            Ball ball = Game.Components.OfType<Ball>().FirstOrDefault()
                        ?? throw new Exception("E420 - BALL DIDN'T EXIST LOL");

            Position.Y = ball.Position.Y;
        }

        public void ConstrainWithinBounds(Rectangle viewBounds)
        {
            Vector2 boundOpposite = (Size + Position);

            if (Position.X < viewBounds.Left)
                Position.X = viewBounds.Left;

            if (boundOpposite.X > viewBounds.Right)
                Position.X = viewBounds.Right - Size.X;

            if (Position.Y < viewBounds.Top)
                Position.Y = viewBounds.Top;

            if (boundOpposite.Y > viewBounds.Bottom)
                Position.Y = viewBounds.Bottom - Size.Y;
        }
    }
}