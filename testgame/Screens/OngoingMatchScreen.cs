using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Core.Screens;
using testgame.Core;
using testgame.Entities;
using testgame.Entities.GUI;
using testgame.Mechanics;
using testgame.Mechanics.Serve;

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
        private FirstServerFinder FirstServerFinder { get; set; }

        private IMatch _match;
        private IRound _round => _match.CurrentRound;

        public ServeBallHandler ServeBallHandler { get; private set; }

        // Always-active components.
        // TODO: Add always-active components.

        public OngoingMatchScreen(Game game) : base(game)
        {
            Translucent = false;

            Components.Add(Ball = new Ball(game));
            Components.Add(AiPaddle = new Paddle(game, Team.Red));
            Components.Add(PlayerPaddle = new Paddle(game, Team.Blue));
            Components.Add(ScoreDisplay = new ScoreDisplay(game));
            Components.Add(FirstServerFinder = new FirstServerFinder(Game, Ball));
            Components.Add(ServeBallHandler = new ServeBallHandler(Game));
        }

        public override void Initialize()
        {
            base.Initialize();

            _match = Game.Services.GetService<IMatch>();
            _match.MatchStateChanges += onMatchStateChanges;
        }

        protected override void LoadContent()
        {
            debugFont = Game.Content.Load<SpriteFont>("Arial");
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
            }
        }

        /// <summary>
        /// ROUND EVENTS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            // TODO
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