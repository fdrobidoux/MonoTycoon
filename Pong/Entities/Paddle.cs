using System;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTycoon;
using MonoTycoon.Graphics.Primitives;
using MonoTycoon.Physics;
using MonoTycoon.States;
using Pong.Core;
using Pong.Mechanics;

namespace Pong.Entities
{
    public class Paddle : DrawableGameComponent
    {
        private const int KB_SPEED_PIXELS_PER_SECOND = 250;

        public Transform2 Transform { get; set; }

        public Texture2D PaddleTexture { get; private set; }
        public Team Team;

        Texture2D debugTexture;

        public Paddle(Game game, Team team) : base(game)
        {
            Team = team;
            Transform = Transform2.Zero;
        }

        public override void Initialize()
        {
            //IMatch match = Game.Services.GetService<IMatch>();
            //match.StateChanges += OnMatchStateChanges;

            //IRound round = match.CurrentRound;
            //if (round != null)
            //    round.StateChanges += OnRoundStateChanges;

            Transform.Size = new Size2(50, 100);

            float x;
            switch (Team)
            {
                case Team.Blue:
                    x = GraphicsDevice.Viewport.Bounds.Left;
                    break;
                case Team.Red:
                    x = GraphicsDevice.Viewport.Bounds.Right - Transform.Size.Width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Team), Team, null);
            }
            
            Transform.Location = new Vector2(x, (GraphicsDevice.Viewport.Height) - (Transform.Size.Height) / 2f);

            Visible = true;
            Enabled = true;

            base.Initialize(); // Will call `LoadContent()`;
        }

        private void OnMatchStateChanges(IMachineStateComponent<MatchState> component, MatchState previous)
        {
            if (!(component is IMatch match))
                return;

            //_moving = (e.Modified.Any(MatchState.InProgress, MatchState.DemoMode));

            if (match.State == MatchState.FindingFirstServer)
            {
                match.CurrentRound.StateChanges += OnRoundStateChanges;
            }
        }

        private void OnRoundStateChanges(IMachineStateComponent<RoundState> component, RoundState e)
        {
			if (!(component is IRound round))
				return;

            if (round.State == RoundState.NotStarted)
            {
                Enabled = false;
                Visible = false;
            }
            else if (round.State == RoundState.WaitingForBallServe)
            {
                Visible = true;
                Enabled = true;
            }
        }

        protected override void LoadContent()
        {
            // TODO: Change sprite for paddle.
            PaddleTexture = Game.Content.Load<Texture2D>("textures/ship");
            debugTexture = RectangleTexture.Create(Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            IMatch match = Game.Services.GetService<IMatch>();

            if (Team == Team.Red || match.State == MatchState.DemoMode)
                MoveAI(gameTime);
            else if (Team == Team.Blue)
                //MoveWithMouse(gameTime);
                MoveWithKeyboard(gameTime);
            else
                throw new NotImplementedException();

            ConstrainWithinBounds(GraphicsDevice.Viewport.Bounds);
        }

        private void MoveWithKeyboard(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            bool upPressed = state.IsKeyDown(Keys.Up);
            bool downPressed = state.IsKeyDown(Keys.Down);

            if (!(upPressed && downPressed))
            {
                if (upPressed)
                {
                    Transform.Location = new Vector2(
                        x: Transform.Location.X,
                        y: (float)(Transform.Location.Y - (KB_SPEED_PIXELS_PER_SECOND * gameTime.ElapsedGameTime.TotalSeconds))
                    );
                }
                else if (downPressed)
                {
                    Transform.Location = new Vector2(
                        x: Transform.Location.X,
                        y: (float)(Transform.Location.Y + (KB_SPEED_PIXELS_PER_SECOND * gameTime.ElapsedGameTime.TotalSeconds))
                    );
                }
            }
        }
            
        private void MoveWithMouse(GameTime gameTime)
        {
            Transform.Location = new Vector2(
                x: Transform.Location.X,
                y: Mouse.GetState().Y - (Transform.Size.Height / 2)
            );
        }

        private void MoveAI(GameTime gameTime)
        {
            // TODO: Rewrite this.
        }

        public override void Draw(GameTime gameTime)
        {
            var paddleRectangle = this.Transform.ToRectangle();
            Game.GetSpriteBatch().Draw(PaddleTexture, paddleRectangle, null, Color.White);
            Game.GetSpriteBatch().Draw(debugTexture, Transform.ToRectangle().Center.ToVector2(), null, Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 1f);
            var rec = Transform.ToRectangle();
            rec.Inflate(1f, 1f);
            Game.GetSpriteBatch().Draw(debugTexture, rec, Color.White);
        }

        public void ConstrainWithinBounds(Rectangle viewBounds)
        {
            //Vector2 boundOpposite = (Size + Position);

            Transform.Location = new Vector2(
                MathHelper.Clamp(Transform.Location.X, viewBounds.Left, viewBounds.Right - Transform.Size.Width),
                MathHelper.Clamp(Transform.Location.Y, viewBounds.Top, viewBounds.Bottom - Transform.Size.Height)
            );

            //if (Position.X < viewBounds.Left)
            //    Position.X = viewBounds.Left;

            //if (boundOpposite.X > viewBounds.Right)
            //    Position.X = viewBounds.Right - Size.X;

            //if (Position.Y < viewBounds.Top)
            //    Position.Y = viewBounds.Top;

            //if (boundOpposite.Y > viewBounds.Bottom)
            //    Position.Y = viewBounds.Bottom - Size.Y;
        }
    }
}