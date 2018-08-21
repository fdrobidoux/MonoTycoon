using System;

namespace testgame.Mechanics
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
    }
}