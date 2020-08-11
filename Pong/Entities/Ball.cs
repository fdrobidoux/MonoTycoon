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
using MonoTycoon.Graphics.Primitives;

namespace Pong.Entities
{
	public class Ball : DrawableGameComponent, IMatchStateSensitive, IRoundStateSensitive
	{
		private const double STARTING_VELOCITY = 600; // Pixels per second.

		public Transform2 Transform { get; set; }

		public double Velocity = STARTING_VELOCITY;
		public Vector2 Direction { get; set; } = Vector2.UnitX;

		public Texture2D Sprite;

		Texture2D debugTexture;

#if DEBUG
        public SpriteFont DebugFont { get; private set; }
#endif

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
			Direction = Vector2.Normalize(new Vector2(1, 2));

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

			debugTexture = RectangleTexture.Create(Color.DeepPink);
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
			Game.GetSpriteBatch().Draw(debugTexture, Transform.ToRectangle(), Color.White);
#endif
		}

		private void Bounce(GameTime gt, Rectangle bounds)
		{
			float diffX, diffY;

			Transform.DeconstructScaledF(out Vector2 locationF, out Vector2 sizeF);

			locationF -= sizeF * 0.5f;

			if ((diffX = locationF.X - bounds.Left) <= 0f)
			{
				Direction *= new Vector2(-1f, 1f);
				Transform -= new Vector2(diffX, 0f);
			}
			else if ((diffX = bounds.Right - (locationF + sizeF).X) <= 0f)
			{
				Direction *= new Vector2(-1f, 1f);
				Transform += new Vector2(diffX, 0f);
			}
			if ((diffY = locationF.Y - bounds.Top) <= 0f)
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
				MathHelper.Clamp(Transform.Location.X, viewBounds.Left, viewBounds.Right - Transform.Size.Width),
				MathHelper.Clamp(Transform.Location.Y, viewBounds.Top, viewBounds.Bottom - Transform.Size.Height)
			);
		}

		private Rectangle centerDivide()
		{
			Rectangle rect = Transform.ToRectangle();
			rect.Offset(rect.Size.DivideBy(2).Scale(-1f));
			return rect;
		}

        public void StateChanged(IMachineStateComponent<MatchState> component, MatchState previousState)
        {
            if (!(component is Match match)) return;

            Visible = match.State.Any(MatchState.FindingFirstServer, MatchState.InProgress);
        }

        public void StateChanged(IMachineStateComponent<RoundState> component, RoundState previousState)
        {
            if (!(component is IRound round)) return;

            Visible = !round.State.Equals(RoundState.NotStarted);
			Enabled = round.State.Equals(RoundState.InProgress);

			if (round.State.Equals(RoundState.WaitingForBallServe))
			{
				Transform.Scale = 1f;
				Visible = true;
			}
        }
    }
}