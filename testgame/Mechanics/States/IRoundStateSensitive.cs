using System;

namespace Pong.Mechanics.States
{
	public interface IRoundStateSensitive : IStateSensitive<IRound, RoundState> 
	{
		new void StateChanged(IRound round, RoundState previous);
	}
}
