using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.States
{
	public interface IMachineStateComponent<out T> : IGameComponent, IDisposable
		where T : Enum, IComparable
	{
		T State { get; }
		event MachineStateEventHandler<IMachineStateComponent<T>, T> StateChanges;
		Type GetSensitivityType();
	}
}
