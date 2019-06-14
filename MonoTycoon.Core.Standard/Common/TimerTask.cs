using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.Common
{
	public sealed class TimerTask : IUpdateable
	{
		readonly Action _performTask;
		double _intervalMs;

		public bool Enabled { get; set; }
		public int UpdateOrder => 0;

		[Obsolete("Not used for this component.")]
		public event EventHandler<EventArgs> EnabledChanged;
		[Obsolete("Not used for this component.")]
		public event EventHandler<EventArgs> UpdateOrderChanged;

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
		public bool IsFinished { get; private set; }
		public double ElapsedMs { get; private set; }

		public TimerTask(Action performTask, double intervalMs, bool recurring = true)
		{
			_performTask = performTask;
			_intervalMs = intervalMs;
			Recurring = recurring;
		}

		public void Update(TimeSpan delta)
		{
			if (!Enabled)
				return;

			if (IsFinished)
				return;

			ElapsedMs += delta.TotalMilliseconds;
			if (!(ElapsedMs > _intervalMs))
				return;

			_performTask();
			IsFinished = true;
			if (Recurring)
				Reset();
		}

		public void Update(GameTime gameTime) => Update(gameTime.ElapsedGameTime);

		/// <summary>
		///     Resets the timer, and sets the timer as unfinished.
		/// </summary>
		/// <param name="modEnabled">A <see langword="bool"/> if it's necessary to change enabled status, or <see langword="null"/> if otherwise.</param>
		public void Reset(bool? modEnabled = null)
		{
			ElapsedMs = 0;
			IsFinished = false;

			if (modEnabled is bool en)
				Enabled = en;
		}
	}
}