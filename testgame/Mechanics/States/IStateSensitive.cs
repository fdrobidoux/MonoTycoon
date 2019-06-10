using Microsoft.Xna.Framework;
using System;

namespace Pong.Mechanics.States
{
    public interface IStateSensitive<TSystem, in TEnum>
        where TSystem : IGameComponent
        where TEnum : Enum
    {
        void StateChanged(TSystem component, TEnum previousState);
    }
}
