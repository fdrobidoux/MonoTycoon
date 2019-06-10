using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pong.Mechanics.States
{
	public interface IMachineStateComponent<T> : IGameComponent, IDisposable
		where T : Enum
	{
		T State { get; set; }
		event MachineStateEventHandler<IMachineStateComponent<T>, T> StateChanges;
		Type GetSensitivityType();
	}
}
