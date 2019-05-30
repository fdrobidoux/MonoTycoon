using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Core.Screens;
using System;
using testgame.Entities;
using testgame.Mechanics;

namespace testgame.Screens
{
    public class OngoingMatchScreen : Screen
    {
        private SpriteFont debugFont;

        // Game Components
        private Paddle PlayerPaddle { get; set; }
        private Paddle AiPaddle { get; set; }
        private Ball Ball { get; set; }

        private IMatch _match;
        private IRound _round => _match.CurrentRound;

        // Always-active components.
        // TODO: Add always-active components.

        public OngoingMatchScreen(Game game) : base(game)
        {
            Translucent = false;
            Components.Add(PlayerPaddle = new Paddle(game, Team.Blue));
            Components.Add(AiPaddle = new Paddle(game, Team.Red));
            Components.Add(Ball = new Ball(game));
        }

        public override void Initialize()
        {
            base.Initialize();
            _match = Game.Services.GetService<IMatch>();
        }

        protected override void LoadContent()
        {
            debugFont = Game.Content.Load<SpriteFont>("Arial");
            base.LoadContent();
        }

        public override void Activated()
        {
            
        }

        public override void Deactivated()
        {

        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
#if DEBUG
            var spriteBatch = Game.GetSpriteBatch();
            spriteBatch.DrawString(this.debugFont, $"MatchState: {Enum.GetName(typeof(MatchState), this._match.State)}", Vector2.Zero,
                Color.Salmon, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            
            var roundState = (this._round != null) ? Enum.GetName(typeof(RoundState), this._round.State) : "NULL";
            spriteBatch.DrawString(this.debugFont, $"RoundState: {roundState}", new Vector2(0f, 20f),
                Color.YellowGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
#endif
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Ball?.Dispose();
                PlayerPaddle?.Dispose();
                AiPaddle?.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}