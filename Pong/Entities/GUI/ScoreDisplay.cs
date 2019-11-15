using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Mechanics;
using MonoTycoon.Graphics;
using Pong.Core;
using MonoTycoon.States;

namespace Pong.Entities.GUI
{
    public class ScoreDisplay : DrawableGameComponent, IMatchStateSensitive
    {
        private SpriteFont _font;
        private IMatch _match;
        private const string STR_FORMAT = "Team {0}: {1}";

        public ScoreDisplay(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            _match = Game.Services.GetService<IMatch>();
            _match.StateChanges += StateChanged;
            ;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _font = Game.Content.Load<SpriteFont>("fonts/Arial");
        }

        private readonly MatchState[] STATES_WHEN_DISABLED = { MatchState.NotStarted, MatchState.DemoMode };

        private string GenerateTeamText(Team team)
        {
            return string.Format(STR_FORMAT, team, _match.GetScore(team));
        }

        public override void Draw(GameTime gt)
        {
            var sb = Game.GetSpriteBatch();
            Vector2 position;
            float yPos;
            foreach (Team team in Enum.GetValues(typeof(Team)))
            {
                var measure = _font.MeasureString(GenerateTeamText(team));
                yPos = (float) Math.Round(Game.GraphicsDevice.Viewport.Bounds.Height * 0.05F);

                switch (team.GetScreenPosition())
                {
                    case Direction.Left:
                        position = new Vector2(Game.GraphicsDevice.Viewport.Bounds.Width * 0.01F, yPos);
                        break;
                    case Direction.Right:
                        position = new Vector2((Game.GraphicsDevice.Viewport.Bounds.Width * 0.99F) - measure.X, yPos);
                        break;
                    default:
                        position = Vector2.Zero;
                        break;
                }

                sb.DrawString(_font, GenerateTeamText(team), position, team.ToColor());
            }
        }

        public void StateChanged(IMachineStateComponent<MatchState> match, MatchState previousState)
        {
            Enabled = (!match.State.Any(STATES_WHEN_DISABLED));
        }
    }
}
