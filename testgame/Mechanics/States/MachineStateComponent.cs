using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pong.Mechanics.States
{
	public abstract class MachineStateComponent<TStateEnum> : GameComponent, IMachineStateComponent<TStateEnum>
		where TStateEnum : Enum
	{
		TStateEnum _state;

		public TStateEnum State
		{
			get => _state;
			set
			{
				if (value.Equals(_state)) 
					return;
				TStateEnum old = _state;
				_state = value;
				StateChanges?.Invoke(this, old);
			}
		}

		public event MachineStateEventHandler<IMachineStateComponent<TStateEnum>, TStateEnum> StateChanges;

		public MachineStateComponent(Game game) : base(game)
		{
		}

		public override void Initialize()
		{
			base.Initialize();
			
			StateChanges = default;
			StateChanges += WhenOwnStateChanges;
			_state = default;
		}

		private void WhenOwnStateChanges(IMachineStateComponent<TStateEnum> sender, TStateEnum previous) 
			=> WhenOwnStateChanges(previous);

		protected virtual void WhenOwnStateChanges(TStateEnum previous)
		{
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			StateChanges = null;
		}

		public abstract Type GetSensitivityType();
	}
}
