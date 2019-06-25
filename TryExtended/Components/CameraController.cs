using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoTycoon;
using MonoGame.Extended;

namespace TryExtended.Components
{
	public class CameraController : DrawableGameComponent
	{
		public Camera2D Camera { get; private set; }
		public Vector2 direction;
		public float acceleration = 100f;

		public float scrollValue;

		private int _zoomDirection;
		private float _zoomSpeed = 0.5f;
		private float _targetZoom;
		private float _zoomAmount;

		private double elapsed;

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

		private void UpdatePosition(GameTime gt, KeyboardState kb)
		{
			direction = Vector2.Zero;

			// Y Axis
			if (kb.IsKeyDown(Keys.Up))
				direction -= Vector2.UnitY;
			else if (kb.IsKeyDown(Keys.Down))
				direction += Vector2.UnitY;

			// X Axis
			if (kb.IsKeyDown(Keys.Left))
				direction -= Vector2.UnitX;
			else if (kb.IsKeyDown(Keys.Right))
				direction += Vector2.UnitX;

			Camera.Move(direction * (float)(acceleration * gt.ElapsedGameTime.TotalSeconds));
		}

		private void UpdateZoom(GameTime gt, MouseState mouse)
		{
			float zoomAmount;
			int compared = scrollValue.CompareTo(mouse.ScrollWheelValue);

			if (compared != 0)
			{
				_zoomDirection = compared;
				_zoomAmount = _zoomSpeed;
				_targetZoom = Camera.Zoom + ((mouse.ScrollWheelValue - scrollValue) / 120);
				scrollValue = mouse.ScrollWheelValue;
				elapsed = gt.ElapsedGameTime.TotalSeconds;
			}

			if (_zoomDirection != 0)
			{
				if (_zoomDirection == -1)
					Camera.ZoomOut(MathHelper.SmoothStep(_zoomAmount, _targetZoom, (float) elapsed));
				else if (_zoomDirection == 1)
					Camera.ZoomIn(MathHelper.SmoothStep(_targetZoom, _zoomAmount, (float) elapsed));

				if (Camera.Zoom == _targetZoom)
				{
					_zoomDirection = 0;
				}

				elapsed += gt.ElapsedGameTime.TotalMilliseconds;
			}
		}

		public override void Draw(GameTime gt)
		{

		}
	}
}
