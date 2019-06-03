using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTycoon.Core.Screens;
using testgame.Core;
using testgame.Mechanics;
using testgame.Screens;
using testgame.Entities.GUI;

namespace testgame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch SpriteBatch { get; private set; }
        
        public float TotalSecondsCountdown;

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
            Services.AddService<IMatch>(Match = new Match(this, MatchState.NotStarted));

            // ScreenManager Service
            Services.AddService<IScreenManager>(ScreenManager = new ScreenManager(this, new OngoingMatchScreen(this)));
            Components.Add(ScreenManager);
        }

        protected override void Initialize()
        {
            base.Initialize();
            // TODO: Remove those parts when Menu is done.
            Match.State = MatchState.NotStarted;
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
            // TODO: use this.Content to load your game content here
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