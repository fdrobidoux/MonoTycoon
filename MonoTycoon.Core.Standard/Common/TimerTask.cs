using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon
{
	public sealed class TimerTask : IUpdateable
	{
		readonly Action performTask;
		double _intervalMs;

		#region "Basic implementation of IUpdateable"
		private bool _enabled;
		public bool Enabled
		{
			get => _enabled;
			set
			{
				if (_enabled != value)
				{
					_enabled = value;
					EnabledChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}
		private int _updateOrder;
		public int UpdateOrder
		{
			get => _updateOrder;
			set
			{
				if (_updateOrder != value)
				{
					_updateOrder = value;
					UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}
		[Obsolete("Not used for this component.")]
		public event EventHandler<EventArgs> EnabledChanged;
		[Obsolete("Not used for this component.")]
		public event EventHandler<EventArgs> UpdateOrderChanged;
		#endregion

		public double IntervalMs
		{
			get => _intervalMs;
			set
			{
				_intervalMs = value;
				Reset();
			}
		}
		public bool Recurring { get; set; }
		public bool Cumulative { get; set; }
		public bool IsFinished { get; private set; }
		public double ElapsedMs { get; private set; }

		public TimerTask(Action performTask, double intervalMs, bool recurring = true)
		{
			this.performTask = performTask;
			IntervalMs = intervalMs;
			Recurring = recurring;
			Cumulative = true;
		}

		public void Update(GameTime gameTime) => Update(gameTime.ElapsedGameTime);

		public void Update(TimeSpan delta)
		{
			if (!Enabled)
				return;

			if (IsFinished)
				return;

			ElapsedMs += delta.TotalMilliseconds;

			if (ElapsedMs > _intervalMs)
			{
				ElapsedMs -= _intervalMs;

				// Perform the task.
				performTask?.Invoke();

				IsFinished = true;

				if (Recurring)
				{
					Reset();
				}
			}
		}

		/// <summary>
		///     Resets the timer, and sets the timer as unfinished.
		/// </summary>
		/// <param name="modEnabled">A <see langword="bool"/> if it's necessary to change enabled status, or <see langword="null"/> if otherwise.</param>
		public void Reset(bool? modEnabled = null)
		{
			IsFinished = false;

			if (!Cumulative)
				ElapsedMs = 0;

			if (modEnabled is bool en)
				Enabled = en;
		}
	}
}