using Pong.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Mechanics
{
    public interface IRoundStateSensitive
    {
        void Round_StateChanged(IRound round, RoundState previous);
    }
}
