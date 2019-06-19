using Microsoft.Xna.Framework;
using MonoTycoon.Graphics;
using System;

namespace Pong.Mechanics
{
    public enum Team
    {
        Blue,
        Red
    }

    public static class TeamExtensions
    {
        public static Team Opposite(this Team team)
        {
            switch (team)
            {
                case Team.Blue: return Team.Red;
                case Team.Red: return Team.Blue;
                default: throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }

        public static Color ToColor(this Team team)
        {
            switch (team)
            {
                case Team.Blue: return Color.Blue;
                case Team.Red: return Color.Red;
                default: return Color.Gray;
            }
        }

        public static Direction GetScreenPosition(this Team team)
        {
            switch (team)
            {
                case Team.Blue: return Direction.Left;
                case Team.Red: return Direction.Right;
                default: throw new Exception(string.Format("Can't find a direction for Team {0}", team.AsString()));
            }
        }
    }
}