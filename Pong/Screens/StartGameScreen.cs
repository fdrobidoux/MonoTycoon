using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTycoon.Screens;
using System;
using System.Collections.Generic;
using Pong.Mechanics;

namespace Pong.Screens
{
    class StartGameScreen : Screen
    {
        private Match _match;
        private SpriteFont _textFont;
        const string TEXT_ENTER = "Press Enter to start !";

        public StartGameScreen(Game game) : base(game)
        {
            Translucent = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            _match = (Match)Game.Services.GetService<IMatch>();
        }

        protected override void LoadContent()
        {
            _textFont = Game.Content.Load<SpriteFont>("fonts/Arial");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (Manager.ActiveScreen == this)
                {
                    Manager.Pop();
                    _match.StartNewRound();
                    this.Enabled = false;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var textSize = _textFont.MeasureString(TEXT_ENTER);
            var position = (Game.GraphicsDevice.Viewport.Bounds.Size.ToVector2() - textSize) / 2;

            Game.GetSpriteBatch().DrawString(_textFont, TEXT_ENTER, position, Color.DarkRed, 0.0f, Vector2.Zero, 1, SpriteEffects.None, 1.0f);
            
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
