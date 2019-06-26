using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoTycoon;
using MonoGame.Extended;

namespace TryExtended.Components
{
	public partial class CameraController : DrawableGameComponent
	{
		public Camera2D Camera { get; private set; }

		public CameraController(Game game, Camera2D camera) : base(game)
		{
			Camera = camera;
		}

		public override void Initialize()
		{
			_zoomDirection = 0;
			base.Initialize();
		}

		public override void Update(GameTime gt)
		{
			KeyboardState kb = Keyboard.GetState();
			MouseState mouse = Mouse.GetState();

			UpdatePosition(gt, kb);
			UpdateZoom(gt, mouse);
		}

		public override void Draw(GameTime gt)
		{

		}
	}
}
