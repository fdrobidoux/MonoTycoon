using Microsoft.Xna.Framework;
using MonoTycoon.Core;
using System;
using System.Collections.Generic;
using System.Text;
using testgame.Entities;
using testgame.Mechanics.AI;

namespace testgame.Components
{
    public class AIController : GameComponent
    {
        private Paddle controlledPaddle;
        private Ball subjectBall;

        private IEnumerator<AIAction> thinkEnumerator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game">Game instance</param>
        /// <param name="controlledPaddle">Paddle to control</param>
        /// <param name="subjectBall"></param>
        public AIController(Game game, Paddle controlledPaddle, Ball subjectBall) : base(game)
        {
            this.controlledPaddle = controlledPaddle;
            this.subjectBall = subjectBall;

            this.thinkEnumerator = ThinkIterator().GetEnumerator();
        }

        protected IEnumerable<AIAction> ThinkIterator()
        {
            while (true)
            {
                yield return AIAction.WAIT;
            }
        }

        public override void Update(GameTime gt)
        {
            thinkEnumerator.MoveNext();
        }
    }
}
