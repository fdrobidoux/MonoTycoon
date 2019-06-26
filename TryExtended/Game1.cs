using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoTycoon;
using TryExtended.Components;

namespace TryExtended
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : TycoonGame
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		CameraController cameraController;

		public Game1()
		{
			Content.RootDirectory = "Content";
			graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 800,
				PreferredBackBufferHeight = 600
			};

			// Moving character.
			Components.Add(new MovingCharacter(this));
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic related content.
		/// Calling base.Initialize will enumerate through any components and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			// Camera component
			var bvpa = new BoxingViewportAdapter(Window, graphics, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			cameraController = new CameraController(this, new Camera2D(bvpa));
			Components.Add(cameraController);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			GameHelper.WorldBatch.Begin(transformMatrix: cameraController.Camera.GetViewMatrix());
			GameHelper.UIBatch.Begin();

			base.Draw(gameTime);

			GameHelper.WorldBatch.End();
			GameHelper.UIBatch.End();
		}
	}
}
