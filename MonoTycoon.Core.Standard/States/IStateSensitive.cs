﻿using Microsoft.Xna.Framework;
using System;

namespace MonoTycoon.States
{
    public interface IStateSensitive<TSystem, TEnum>
        where TSystem : IGameComponent
        where TEnum : Enum
    {
        void StateChanged(TSystem component, TEnum previousState);
    }
}
