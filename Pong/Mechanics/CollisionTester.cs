using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using MonoTycoon.Graphics;
using MonoTycoon.Physics.Extensions;
using Pong.Entities;

namespace Pong.Mechanics
{
    public class CollisionTester : GameComponent
    {
        Ball _ball;
        List<Paddle> _allPaddles;

        Direction currentDirection;
        Paddle currentPaddle;

        readonly double MAX_ANGLE = 3 * Math.PI / 12;

        public CollisionTester(Game game, Ball ball, IEnumerable<Paddle> paddles) : base(game)
        {
            _ball = ball;
            _allPaddles = paddles as List<Paddle>;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_ball.Direction.X == 0.0f)
                return;

            Direction screenDirection = _ball.Direction.X > 0.0f ? Direction.Right : Direction.Left;
            
            if (currentDirection != screenDirection)
            {
                currentDirection = screenDirection;
                currentPaddle = _allPaddles.First(x => x.Team.GetScreenPosition() == currentDirection);
            }

            var ballRectangle = _ball.Transform.ToRectangle();
            var paddleRectangle = currentPaddle.Transform.ToRectangle();
            
            //Point pointIntersect = new Point();
            //if (currentDirection == Direction.Left)
            //{
            //    pointIntersect = new Point(ballRectangle.Left, ballRectangle.Center.Y);
            //}
            //else if (currentDirection == Direction.Right)
            //{
            //    pointIntersect = new Point(ballRectangle.Right, ballRectangle.Center.Y);
            //}

            if (currentPaddle.Transform.Intersects(_ball.Transform))
            {
                var whereOnPaddle = paddleRectangle.Y + (paddleRectangle.Height / 2) - (ballRectangle.Center.Y);
                var diviser = whereOnPaddle / (paddleRectangle.Height * 0.5f);
                var angle = (float)(diviser * MAX_ANGLE);
                var direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));

                direction.Normalize();
                direction *= new Vector2(1, -1);
                _ball.Direction = direction;
            }
        }
    }
}