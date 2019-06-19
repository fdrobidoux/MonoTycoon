using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTycoon;
using MonoTycoon.Screens;
using Pong.Entities;
using Pong.Mechanics;
using Pong.Screens;

namespace Pong
{
    public class Game1 : TycoonGame
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch SpriteBatch { get; private set; }

        // Services
        Match Match { get; }
        ScreenManager ScreenManager { get; }

        // Screens
        OngoingMatchScreen ongoingMatchScreen;
        StartGameScreen startGameScreen;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

			// Match Service
			Match = new Match(this);
			Match.UpdateOrder = (int)RootComponentOrder.MATCH;
			Services.AddService<IMatch>(Match);
            Components.Add(Match);

            // ScreenManager Service
            ScreenManager = new ScreenManager(this, ongoingMatchScreen = new OngoingMatchScreen(this));
            Services.AddService<IScreenManager>(ScreenManager);
            Components.Add(ScreenManager);
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (!(ScreenManager.Peek() is StartGameScreen))
            {
                startGameScreen = new StartGameScreen(this);
                ScreenManager.Push(startGameScreen);
            }
			Debug.WriteLine("Hello");
        }

        protected override void LoadContent()
        {
            Services.AddService(SpriteBatch = new SpriteBatch(graphics.GraphicsDevice));
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            // Exit if player presses Esc.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.AnisotropicWrap, sortMode: SpriteSortMode.Immediate);
            base.Draw(gameTime);
            SpriteBatch.End();
        }

		private enum RootComponentOrder : int
		{
			DEFAULT = 0,
			MATCH = -1,
			ROUND = -2,
		}
    }
}