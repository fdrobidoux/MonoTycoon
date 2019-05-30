using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Core.Extensions;
using testgame.Core;
using testgame.Mechanics;

namespace testgame.Entities
{
    public class Ball : DrawableGameComponent, IActuallyDrawable
    {
        private Vector2 _velocity = Vector2.Zero;
        private readonly float _baseScale = 0.07f;
        private float _scale;

        public Vector2 Position = Vector2.Zero;
        public Rectangle Bounds => new Rectangle(Position.ToPoint(), Sprite.Bounds.Center.Scale(Scale));

        public float Scale
        {
            get => _scale;
            set => _scale = value * _baseScale;
        }

        public Texture2D Sprite;


        public Ball(Game game) : base(game)
        {
            Scale = 1f;
        }

        public override void Initialize()
        {
            Visible = false;

            IMatch _match = Game.Services.GetService<IMatch>();
            IRound round = _match.CurrentRound;
            
            _match.MatchStateChanges += OnMatchStateChanges;
            if (round != null)
                SetRoundEvents(round);

            _velocity = Vector2.Zero;

            base.Initialize();
        }

        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            if (sender is IMatch match)
            {
                if (e.Modified == MatchState.NotStarted)
                {
                    
                }
                else if (e.HasChangedFrom(MatchState.NotStarted, MatchState.InstanciatedRound))
                {
                    SetRoundEvents(match.CurrentRound);
                }
                else if (e.HasChangedFrom(MatchState.InstanciatedRound, MatchState.InProgress))
                {
                    // Round Instanciated -> In Progress
                }
                else if (e.HasChangedFrom(MatchState.InProgress, MatchState.Finished))
                {
                    // In Progress -> Finished
                    UnsetRoundEvents(match.CurrentRound);
                }
                else if (e.Modified == MatchState.DemoMode)
                {
                    // Finished -> DemoMode
                }
            }
        }

        private void SetRoundEvents(IRound round) => round.RoundStateChanges += OnRoundStateChanges;
        private void UnsetRoundEvents(IRound round) => round.RoundStateChanges -= OnRoundStateChanges;

        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (sender is IRound round)
            {
                Visible = e.Modified != RoundState.NotStarted;
                Enabled = e.Modified.Any(RoundState.InProgress);
            }
        }

        public void Reset()
        {
            Position = (GraphicsDevice.Viewport.Bounds.Center.ToVector2() / 2f) -
                       ((Sprite.Bounds.Size.ToVector2() / 2f) * Scale);
        }

        protected override void LoadContent()
        {
            Sprite = Game.Content.Load<Texture2D>("ball");
        }

        protected override void UnloadContent()
        {
            Sprite?.Dispose();
            Sprite = null;
        }

        public override void Update(GameTime gameTime)
        {
            IMatch match = Game.Services.GetService<IMatch>();
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, Game.Services.GetService<SpriteBatch>());
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
    }
}