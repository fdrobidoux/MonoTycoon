using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTycoon.Core.Extensions;
using testgame.Core;
using testgame.Mechanics;

namespace testgame.Entities
{
    public class Paddle : DrawableGameComponent, IActuallyDrawable
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Size = new Vector2(50, 100);
        public Texture2D Sprite;
        public Team Team;

        public Paddle(Game game, Team team) : base(game)
        {
            Team = team;
        }

        public override void Initialize()
        {
            IMatch match = Game.Services.GetService<IMatch>();
            match.TeamScores += OnTeamScores;
            match.MatchStateChanges += OnMatchStateChanges;
            
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
                if (e.HasChangedFrom(MatchState.DemoMode, MatchState.NotStarted))
                {
                    // TODO: When changed to not started.
                }
                else if (e.HasChangedFrom(MatchState.NotStarted, MatchState.InstanciatedRound))
                {
                    match.CurrentRound.RoundStateChanges += OnRoundStateChanges;
                }
                else if (e.HasChangedFrom(MatchState.InstanciatedRound, MatchState.InProgress))
                {
                    
                }
                else if (e.HasChangedFrom(MatchState.InProgress, MatchState.Finished))
                {
                    
                }
                else if (e.HasChangedFrom(MatchState.Finished, MatchState.DemoMode))
                {
                    
                }
            }
        }

        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (sender is IRound round)
            {
                if (e.HasChangedFrom(RoundState.NotStarted, RoundState.InProgress))
                {
                    
                }
                else if (e.Modified == RoundState.InProgress)
            }
        }

        protected override void LoadContent()
        {
            Sprite = Game.Content.Load<Texture2D>("ship");
        }
        protected override void UnloadContent()
        {
            Sprite.Dispose();
            Sprite = null;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Rectangle(Position.ToPoint(), Size.ToPoint()), null, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            switch (this.Team)
            {
                case Team.Blue:
                    MoveWithMouse(gameTime);
                    break;
                case Team.Red:
                    MoveAI(gameTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ConstrainWithinBounds(GraphicsDevice.Viewport.Bounds);
        }

        private void MoveWithMouse(GameTime gameTime)
        {
            Position.Y = Mouse.GetState().Y;
        }

        private void MoveAI(GameTime gameTime)
        {
            
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