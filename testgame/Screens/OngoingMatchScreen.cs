using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Core.Screens;
using testgame.Core;
using testgame.Entities;
using testgame.Entities.GUI;
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
        private ScoreDisplay ScoreDisplay { get; set; }
        private FirstServerFinderEntity FirstServerFinderEntity { get; set; }

        private IMatch _match;
        private IRound _round => _match.CurrentRound;

        // Always-active components.
        // TODO: Add always-active components.

        public OngoingMatchScreen(Game game) : base(game)
        {
            Translucent = false;
            
            Ball = new Ball(game);

            Components.Add(FirstServerFinderEntity = new FirstServerFinderEntity(Game, Ball));
            Components.Add(ScoreDisplay = new ScoreDisplay(game));
            Components.Add(AiPaddle = new Paddle(game, Team.Red));
            Components.Add(PlayerPaddle = new Paddle(game, Team.Blue));
            Components.Add(Ball);
        }

        public override void Initialize()
        {
            base.Initialize();

            FirstServerFinderEntity.Enabled = false;
            FirstServerFinderEntity.Visible = false;
            
            _match = Game.Services.GetService<IMatch>();
            _match.MatchStateChanges += OnMatchStateChanges;
        }

        protected override void LoadContent()
        {
            debugFont = Game.Content.Load<SpriteFont>("Arial");
        }
        
        public override void Update(GameTime gameTime) 
            => base.Update(gameTime);

        /// <summary>
        /// MATCH EVENTS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            IMatch match = Game.Services.GetService<IMatch>();

            if (e.HasChangedFrom(MatchState.NotStarted, MatchState.InstanciatedRound))
            {
                match.CurrentRound.RoundStateChanges += OnRoundStateChanges;
                FirstServerFinderEntity.Enabled = true;
                FirstServerFinderEntity.Visible = true;
            }
        }

        /// <summary>
        /// ROUND EVENTS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (e.HasChangedFrom(RoundState.NotStarted, RoundState.WaitingForBallServe))
            {
                //if (Components.Contains(FirstServerFinderEntity)) 
                //    Components.Remove(FirstServerFinderEntity);
                FirstServerFinderEntity.Enabled = false;
                FirstServerFinderEntity.Visible = false;
            }
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);
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
            base.Dispose(disposing);

            if (disposing)
            {
                Ball?.Dispose();
                PlayerPaddle?.Dispose();
                AiPaddle?.Dispose();
            }
        }
    }
}