using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Common
{
	public sealed class TimerTask : IUpdateable
	{
		readonly Action performTask;
		double intervalMs;

		#region "Basic IUpdateable implementation"
		int updateOrder;
		private bool enabled;

		public bool Enabled
		{
			get => enabled; set => enabled = value;
		}
		public int UpdateOrder
		{
			get => updateOrder; set => updateOrder = value;
		}
		#endregion

		[Obsolete("Not used for this component.")]
		public event EventHandler<EventArgs> EnabledChanged;
		[Obsolete("Not used for this component.")]
		public event EventHandler<EventArgs> UpdateOrderChanged;

		public double IntervalMs
		{
			get => intervalMs;
			set
			{
				intervalMs = value;
				Reset();
			}
		}
		public bool Recurring { get; set; }
		public bool IsFinished { get; private set; }
		public double ElapsedMs { get; private set; }

		public TimerTask(Action performTask, double intervalMs, bool recurring = true)
		{
			this.performTask = performTask;
			this.intervalMs = intervalMs;
			Recurring = recurring;
		}

		public void Update(GameTime gameTime) => Update(gameTime.ElapsedGameTime);

		public void Update(TimeSpan delta)
		{
			if (!Enabled)
				return;

			if (IsFinished)
				return;

			ElapsedMs += delta.TotalMilliseconds;

			if (ElapsedMs > intervalMs)
			{
				ElapsedMs -= intervalMs;

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
		public void Reset(bool keepLastQuotient = false, bool? modEnabled = null)
		{
			IsFinished = false;

			if (!keepLastQuotient)
				ElapsedMs = 0;

			if (modEnabled is bool en)
				Enabled = en;
		}
	}
}