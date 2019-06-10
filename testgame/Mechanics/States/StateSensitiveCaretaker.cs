using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Mechanics.States
{
    public abstract class StateSensitiveCaretaker<TSystem, TEnumState>
        where TSystem : IGameComponent, IUpdateable
        where TEnumState : Enum
    {
        


    }
}
