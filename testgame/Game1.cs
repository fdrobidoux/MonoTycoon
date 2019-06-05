using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTycoon.Core.Screens;
using testgame.Core;
using testgame.Mechanics;
using testgame.Screens;

namespace testgame
{
    public class Game1 : MonoTycoon.Core.TycoonGame
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch SpriteBatch { get; private set; }

        // Services
        private Match Match { get; }
        private ScreenManager ScreenManager { get; }

        // Screens
        private OngoingMatchScreen ongoingMatchScreen;
        private StartGameScreen startGameScreen;

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
            Services.AddService<IMatch>(Match = new Match(this, MatchState.DemoMode));
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
        }

        protected override void LoadContent()
        {
            // Sprite batch.
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

            SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.AnisotropicWrap);
            base.Draw(gameTime);
            SpriteBatch.End();
        }
    }
}