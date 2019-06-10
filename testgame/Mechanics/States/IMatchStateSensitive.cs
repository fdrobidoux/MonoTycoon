using System;

namespace Pong.Mechanics.States
{
	public interface IMatchStateSensitive : IStateSensitive<IMatch, MatchState> 
	{
		new void StateChanged(IMatch match, MatchState previous);
	}
}
