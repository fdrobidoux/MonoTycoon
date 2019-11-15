using MonoTycoon.States;

namespace Pong.Mechanics
{
	public interface IMatchStateSensitive : IStateSensitive<MatchState>
	{
        //void StateChanged(IMatch match, MatchState previousState);
        //void IStateSensitive<MatchState>.StateChanged(IMachineStateComponent<MatchState> component, MatchState previousState)
        //    => StateChanged((IMatch)component, previousState);
	}
}
