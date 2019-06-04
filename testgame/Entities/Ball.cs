using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Core;
using MonoTycoon.Core.Physics;
using testgame.Core;
using testgame.Mechanics;

namespace testgame.Entities
{
    public class Ball : DrawableGameComponent
    {
        private const double STARTING_VELOCITY = 100d;

        public double Velocity = STARTING_VELOCITY;
        public Vector2 Direction = Vector2.UnitX;

        public Transform2 Transform { get; set; }

        public Texture2D Sprite;

#if DEBUG
        public SpriteFont DebugFont { get; private set; }
#endif

        public Ball(Game game) : base(game)
        {
            VisibleChanged += FUCK;
            EnabledChanged += SHIT;
            Transform = Transform2.Zero;
        }

        private void FUCK(object sender, EventArgs e)
        {
            Console.WriteLine("FUCK");
        }

        private void SHIT(object sender, EventArgs e)
        {
            Console.WriteLine("SHIT");
        }

        public override void Initialize()
        {
            base.Initialize();

            Transform = new Transform2(Vector2.Zero, new Size2(50, 50), 1f);

            IMatch _match = Game.Services.GetService<IMatch>();
            
            _match.MatchStateChanges += OnMatchStateChanges;

            Velocity = STARTING_VELOCITY;
            Direction = Vector2.UnitX;

            Enabled = false;
            Visible = false;
        }

        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            if (sender is IMatch match)
            {
                Visible = e.Modified.Any(MatchState.InstanciatedRound, MatchState.InProgress);

                if (e.Modified == MatchState.InstanciatedRound)
                    SetRoundEvents(match.CurrentRound);
            }
        }
        
        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            Visible = !e.Modified.Equals(RoundState.NotStarted);
            Enabled = e.Modified.Equals(RoundState.InProgress);
        }

        private void SetRoundEvents(IRound round) => round.RoundStateChanges += OnRoundStateChanges;
        //private void UnsetRoundEvents(IRound round) => round.RoundStateChanges -= OnRoundStateChanges;

        public void Reset()
        {
            /*Position = (GraphicsDevice.Viewport.Bounds.Center.ToVector2() / 2f) -
                       ((_baseSize / 2) * Transform.Scale);*/
        }

        protected override void LoadContent()
        {
            Sprite = Game.Content.Load<Texture2D>("ball");
#if DEBUG
            DebugFont = Game.Content.Load<SpriteFont>("arial");
#endif
        }

        public override void Update(GameTime gameTime)
        {
            Transform.Location += Direction * (float)(Velocity * gameTime.ElapsedGameTime.TotalMilliseconds);
            ConstrainWithinBounds(GraphicsDevice.Viewport.Bounds);
        }

        public void ConstrainWithinBounds(Rectangle viewBounds)
        {
            //Vector2 boundOpposite = (Size + Position);

            Transform.Location = new Vector2(
                Math.Clamp(Transform.Location.X, viewBounds.Left, viewBounds.Right - Transform.Size.Width),
                Math.Clamp(Transform.Location.Y, viewBounds.Top, viewBounds.Bottom - Transform.Size.Height)
            );
        }

        public override void Draw(GameTime gameTime)
        {
            var rect = Transform.ToRectangle();
            Game.GetSpriteBatch().Draw(Sprite, rect, Color.White);
#if DEBUG
            var str = $"Transform {Transform.ToRectangle().ToString()}";
            var position = new Vector2(x: Game.GraphicsDevice.Viewport.Width, y: (Game.GraphicsDevice.Viewport.Height - DebugFont.MeasureString(str).Y));
            Game.GetSpriteBatch().DrawString(DebugFont, str, position, Color.Azure, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Console.WriteLine($"Transform {Transform.ToString()}");
#endif 
        }
    }
}