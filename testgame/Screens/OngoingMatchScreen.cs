using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoTycoon.Core.Screens;
using Pong.Core;
using Pong.Entities;
using Pong.Entities.GUI;
using Pong.Mechanics;
using Pong.Mechanics.Serve;
using Pong.Mechanics.States;

namespace Pong.Screens
{
    public class OngoingMatchScreen : Screen, IMatchStateSensitive, IRoundStateSensitive
    {
#if DEBUG
        private SpriteFont debugFont;
#endif
        // Game Components
        Paddle PlayerPaddle { get; set; }
        Paddle AiPaddle { get; set; }
        Ball Ball { get; set; }
        ScoreDisplay ScoreDisplay { get; set; }

        Match _match;
        Round _round => _match.CurrentRound;

        public ServeBallHandler ServeBallHandler { get; private set; }
        private FirstServerFinder FirstServerFinder { get; set; }

        private Song music;

        public OngoingMatchScreen(Game game) : base(game)
        {
            Translucent = false;

            Components.Add(Ball = new Ball(game));
            Components.Add(AiPaddle = new Paddle(game, Team.Red));
            Components.Add(PlayerPaddle = new Paddle(game, Team.Blue));
            Components.Add(ScoreDisplay = new ScoreDisplay(game));
            Components.Add(FirstServerFinder = new FirstServerFinder(Game, Ball));
            Components.Add(ServeBallHandler = new ServeBallHandler(Game));
        }

        public override void Initialize()
        {
            _match = Game.Services.GetService<Match>();
			_match.StateChanges += StateChanged;

			MediaPlayer.Volume = 0.5f;

			//GameComponentCollection coll = new GameComponentCollection();

            base.Initialize();
        }

		protected override void LoadContent()
        {
		#if DEBUG
            debugFont = Game.Content.Load<SpriteFont>("fonts/Arial");
		#endif
            music = Game.Content.Load<Song>("music/ingame");
        }

		private void OnMatchStateChanged(IMatch sender, MatchState previous)
		{
			if (sender is IMatch match)
			{
				StateChanged(match, previous);
				foreach (var sensitiveComp in Components.OfType<IMatchStateSensitive>())
				{
					sensitiveComp.StateChanged(match, previous);
				}
			}
		}

		private void OnRoundStateChanged(IRound round, RoundState previous)
		{
			
		}

		public void handleSensitivity<T1, T2, T3>(T1 comp, T3 prev) 
			where T1 : IMachineStateComponent<T3>
            where T2 : IStateSensitive<T1, T3>
            where T3 : Enum
        {
			foreach (T2 sensitiveComp in Components.OfType<T2>()) // TODO: Implement `OrderBy()` call.
			{
				sensitiveComp.StateChanged(comp, prev);
			}
        }

		/// <summary>
		/// MATCH EVENTS
		/// </summary>
		public void StateChanged(IMatch match, MatchState previous)
		{
			if (match.State == MatchState.InstanciatedRound)
			{
				match.CurrentRound.StateChanges += StateChanged;
				MediaPlayer.Play(music);
			}
			else if (match.State == MatchState.Finished)
			{
				match.CurrentRound.StateChanges -= StateChanged;
				MediaPlayer.Stop();
			}

			handleSensitivity<IMatch, IMatchStateSensitive, MatchState>(match, previous);
		}

		/// <summary>
		/// ROUND EVENTS
		/// </summary>
		public void StateChanged(IRound round, RoundState previous)
		{
			if (round.State.Equals(RoundState.WaitingForBallServe))
			{
				Paddle servingPaddle = Components.OfType<Paddle>().First(x => x.Team == round.ServingTeam);
				ServeBallHandler.AssignEntitiesNecessaryForServing(Ball, servingPaddle);
			}
			else if (previous.Equals(RoundState.WaitingForBallServe))
			{
				ServeBallHandler.UnassignEntitiesNecessaryForServing();
			}

			handleSensitivity<IRound, IRoundStateSensitive, RoundState>(round, previous);
		}

		public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);
#if DEBUG
            string matchState = Enum.GetName(typeof(MatchState), _match.State);
            Game.GetSpriteBatch().DrawString(debugFont, $"MatchState: {matchState}", Vector2.Zero,
                Color.Salmon, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            string roundState = (this._round != null) ? Enum.GetName(typeof(RoundState), this._round.State) : "NULL";
            Game.GetSpriteBatch().DrawString(debugFont, $"RoundState: {roundState}", new Vector2(0f, 20f),
                Color.YellowGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
#endif
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Ball?.Dispose();
                PlayerPaddle?.Dispose();
                AiPaddle?.Dispose();
                ServeBallHandler?.Dispose();
            }
        }
	}
}