using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Mechanics
{
    public interface IMatchStateSensitive
    {
        void Match_StateChanged(IMatch match, MatchState previous);
    }
}
