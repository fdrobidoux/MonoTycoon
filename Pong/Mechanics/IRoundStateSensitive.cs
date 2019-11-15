using MonoTycoon.States;

namespace Pong.Mechanics
{
    public interface IRoundStateSensitive : IStateSensitive<RoundState>
	{
        //new void StateChanged(IRound component, RoundState previousState);
    }
}
