using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MonoTycoon;
using MonoTycoon.Physics;
using Pong.Core;
using Pong.Mechanics;
using System.Collections.Generic;
using MonoTycoon.States;

namespace Pong.Entities
{
	public class Ball : DrawableGameComponent, IMatchStateSensitive, IRoundStateSensitive
	{
		private const double STARTING_VELOCITY = 300; // Pixels per second.

		public Transform2 Transform { get; set; }

		public double Velocity = STARTING_VELOCITY;
		public Vector2 Direction = Vector2.UnitX;

		public Texture2D Sprite;

		public SpriteFont DebugFont { get; private set; }

		private const string SFX_WALLHIT_BASE = "sfx/wall_hit_{0}";
		private SoundEffect[] sfxGroup_WallHit;

		public Ball(Game game) : base(game)
		{
			Transform = Transform2.Zero;
		}

		public override void Initialize()
		{
			base.Initialize();

			Transform = new Transform2(Vector2.Zero, new Size2(50, 50), 1f);

			Velocity = STARTING_VELOCITY;
			Direction = new Vector2(0.35f, 0.65f);

			Enabled = false;
			Visible = false;
		}

		protected override void LoadContent()
		{
			var theSfx = new List<SoundEffect>();

			Sprite = Game.Content.Load<Texture2D>("textures/ball");
			DebugFont = Game.Content.Load<SpriteFont>("fonts/Arial");

			for (int i = 1; i <= 3; i++)
			{
				theSfx.Add(Game.Content.Load<SoundEffect>(string.Format(SFX_WALLHIT_BASE, i)));
			}
			sfxGroup_WallHit = theSfx.ToArray();
		}

		public void StateChanged(IMatch match, MatchState previousState)
		{
			Visible = match.State.Any(MatchState.FindingFirstServer, MatchState.InProgress);
		}

		public void StateChanged(IRound round, RoundState previous)
		{
			Visible = !round.State.Equals(RoundState.NotStarted);
			Enabled = round.State.Equals(RoundState.InProgress);

			if (round.State.Equals(RoundState.WaitingForBallServe))
			{
				Transform.Scale = 1f;
				Visible = true;
			}
		}

		public override void Update(GameTime gt)
		{
			Transform.Location += Direction * (float)(Velocity * gt.ElapsedGameTime.TotalSeconds);

			Bounce(gt, GraphicsDevice.Viewport.Bounds);

			//ConstrainWithinBounds(GraphicsDevice.Viewport.Bounds);
		}

		public override void Draw(GameTime gameTime)
		{
			Game.GetSpriteBatch().Draw(Sprite, centerDivide(), Color.White);
#if DEBUG
			var str = $"Transform {Transform.ToRectangle().ToString()}";
			var position = new Vector2(x: 0, y: (Game.GraphicsDevice.Viewport.Height - DebugFont.MeasureString(str).Y));
			Game.GetSpriteBatch().DrawString(DebugFont, str, position, Color.Blue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
#endif
		}

		private void Bounce(GameTime gt, Rectangle bounds)
		{
			Vector2 minLocationF;
			float diffX, diffY;

			Transform.DeconstructScaledF(out Vector2 locationF, out Vector2 sizeF);
			sizeF /= 2f;

			minLocationF = locationF - sizeF;

			if ((diffX = minLocationF.X) <= 0f)
			{
				Direction *= new Vector2(-1f, 1f);
				Transform -= new Vector2(diffX, 0f);
			}
			else if ((diffX = bounds.Right - (locationF + sizeF).X) <= 0f)
			{
				Direction *= new Vector2(-1f, 1f);
				Transform += new Vector2(diffX, 0f);
			}
			if ((diffY = minLocationF.Y) <= 0f)
			{
				Direction *= new Vector2(1f, -1f);
				Transform -= new Vector2(0f, diffY);
			}
			else if ((diffY = bounds.Bottom - (locationF + sizeF).Y) <= 0f)
			{
				Direction *= new Vector2(1f, -1f);
				Transform += new Vector2(0f, diffY);
			}

			if (diffY <= 0f || diffX <= 0f)
			{
				sfxGroup_WallHit[new Random().Next(0, sfxGroup_WallHit.Length)].CreateInstance().Play();
			}
		}

		[Obsolete]
		public void ConstrainWithinBounds(Rectangle viewBounds)
		{
			//Vector2 boundOpposite = (Size + Position);

			Transform.Location = new Vector2(
				Math.Clamp(Transform.Location.X, viewBounds.Left, viewBounds.Right - Transform.Size.Width),
				Math.Clamp(Transform.Location.Y, viewBounds.Top, viewBounds.Bottom - Transform.Size.Height)
			);
		}

		private Rectangle centerDivide()
		{
			Rectangle rect = Transform.ToRectangle();
			rect.Location -= rect.Size.DivideBy(2);
			return rect;
		}
	}
}