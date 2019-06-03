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
        public float Velocity = 1.0F;
        public Vector2 Direction = Vector2.UnitX;

        public Transform2 Transform { get; private set; }

        public Texture2D Sprite;

        public Ball(Game game) : base(game)
        {
            Transform = new Transform2(1f);
        }

        public override void Initialize()
        {
            IMatch _match = Game.Services.GetService<IMatch>();
            
            _match.MatchStateChanges += OnMatchStateChanges;

            Velocity = 1.0F;
            Direction = Vector2.UnitX;

            base.Initialize();
        }

        private void OnMatchStateChanges(object sender, ValueChangedEvent<MatchState> e)
        {
            if (sender is IMatch match)
            {
                if (e.Modified == MatchState.InstanciatedRound)
                {
                    SetRoundEvents(match.CurrentRound);
                }
                else if (e.HasChangedFrom(MatchState.InstanciatedRound, MatchState.InProgress))
                {
                    Visible = true;
                }
                else if (e.HasChangedFrom(MatchState.InProgress, MatchState.Finished))
                {
                    // In Progress -> Finished
                }
                else if (e.Modified == MatchState.DemoMode)
                {
                    // Finished -> DemoMode
                }
            }
        }
        
        private void OnRoundStateChanges(object sender, ValueChangedEvent<RoundState> e)
        {
            if (e.Modified != RoundState.NotStarted)
            {
                Visible = true;
            }

            if (e.Modified.Any(RoundState.InProgress))
            {
                Enabled = true;
            }
        }

        private void SetRoundEvents(IRound round) => round.RoundStateChanges += OnRoundStateChanges;
        //private void UnsetRoundEvents(IRound round) => round.RoundStateChanges -= OnRoundStateChanges;

        public void Reset()
        {
            /*Position = (GraphicsDevice.Viewport.Bounds.Center.ToVector2() / 2f) -
                       ((_baseSize / 2) * Transform.Scale);*/
            throw new NotImplementedException();
        }

        protected override void LoadContent()
        {
            Sprite = Game.Content.Load<Texture2D>("ball");
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GetSpriteBatch().Draw(Sprite, Transform.ToRectangle(), Color.White);
        }
    }
}