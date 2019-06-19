using System;
using System.Collections.Generic;

namespace MonoTycoon.States
{
	public delegate void MachineStateEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e)
		where TSender : IMachineStateComponent<TEventArgs>
		where TEventArgs : Enum;
}
