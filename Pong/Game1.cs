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
        public ScreenManager Screens { get; private set; }

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
			var _match = new Match(this) { UpdateOrder = -1 };
			Services.AddService<IMatch>(_match);
            Components.Add(_match);

            // ScreenManager Service
            Screens = new ScreenManager(this, ongoingMatchScreen = new OngoingMatchScreen(this));
            Services.AddService<IScreenManager>(Screens);
            Components.Add(Screens);
        }

        protected override void Initialize()
        {
			if (!(Screens.Peek() is StartGameScreen))
            {
                startGameScreen = new StartGameScreen(this);
                Screens.Push(startGameScreen);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Services.AddService(SpriteBatch = new SpriteBatch(graphics.GraphicsDevice));

            base.LoadContent();
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