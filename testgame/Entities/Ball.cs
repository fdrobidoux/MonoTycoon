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
//        private IMatch _match;
        
        private readonly float _baseScale = 0.07f;
        private float _scale;
        private Vector2 _velocity = Vector2.Zero;
        
        public Vector2 Position = Vector2.Zero;

        public float Scale
        {
            get => _scale;
            set => _scale = value * _baseScale;
        }
        public Texture2D Sprite;

        /// <summary>
        /// Constructor for object `Ball`.
        /// </summary>
        /// <param name="game"></param>
        public Ball(Game game) : base(game)
        {
            Scale = 1f;
        }

        public override void Initialize()
        {
            Visible = false;

            IMatch _match = Game.Services.GetService<IMatch>();
            _match.MatchStateChanges += OnMatchStateChanges;
//            _match.MatchStateChanges += OnMatchStateChanges;

            IRound round;
            if ((round = _match.CurrentRound) != null)
            {
//                round.RoundStateChanges += OnRoundStateChanges;
            }

            _velocity = Vector2.Zero;

            base.Initialize();
        }

        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            if (sender is IMatch match)
            {
                if (e.HasChangedFrom(MatchState.NotStarted, MatchState.InstanciatedRound))
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
                }
                else if (e.HasChangedFrom(MatchState.Finished, MatchState.DemoMode))
                {
                    // Finished -> DemoMode
                }
            }
        }

        private void SetRoundEvents(IRound round)
        {
            
        }

        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (sender is IRound round)
            {
                switch (e.Modified)
                {
                    case RoundState.NotStarted:
                    case RoundState.WaitingForBallServe:
                        break;
                    case RoundState.InProgress:
                        break;
                    case RoundState.Completed:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(e), e, null);
                }
            }
        }

        public void Reset()
        {
            Position = (GraphicsDevice.Viewport.Bounds.Center.ToVector2() / 2f) - ((Sprite.Bounds.Size.ToVector2() / 2f) * Scale);
        }

        protected override void LoadContent()
        {
            Sprite = Game.Content.Load<Texture2D>("ball");
        }

        protected override void UnloadContent()
        {
            Sprite.Dispose();
            Sprite = null;
        }

        public override void Update(GameTime gameTime)
        {
            var match = Game.Services.GetService<IMatch>();

            if (match.State == MatchState.InProgress)
            {
                Position += _velocity * gameTime.Delta();
            }
            else if (match.State == MatchState.InstanciatedRound)
            {
                Reset();
            }
            Console.WriteLine(Position);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
    }

    public struct BallState
    {
        
    }
}