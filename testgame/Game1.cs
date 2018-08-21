using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using testgame.Core;
using testgame.Entities;
using testgame.Mechanics;

namespace testgame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch;

        // Game Components
        private Paddle PlayerPaddle { get; }
        private Paddle AiPaddle { get; }
        private Ball Ball { get; }

        public float TotalSecondsCountdown;
        
        // Services
        private Match Match { get; }
        private Round Round => Match.CurrentRound;
        public IRound ImmutRound => ((IMatch) Match).CurrentRound;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800, 
                PreferredBackBufferHeight = 600
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Pong Game Components
            PlayerPaddle = new Paddle(this, Team.Blue);
            AiPaddle = new Paddle(this, Team.Red);
            Ball = new Ball(this) { Enabled = false };
            Components.Add(PlayerPaddle);
            Components.Add(AiPaddle);
            Components.Add(Ball);
            
            // Match Service
            Services.AddService<IMatch>(Match = new Match(this));
            Components.Add(Match);
        }

        protected override void Initialize()
        {
            base.Initialize();
            // TODO: Remove those parts when Menu is done.
            Match.State = MatchState.NotStarted;
        }

        protected override void LoadContent()
        {
            // Sprite batch.
            SpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            foreach (var updateable in Components.OfType<IUpdateable>())
            {
                updateable.Update(gameTime);
            }
            
            // base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.AnisotropicWrap);
            
            foreach (var actuallyDrawable in Components.OfType<IActuallyDrawable>())
                actuallyDrawable.Draw(gameTime, SpriteBatch);
            
            SpriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}