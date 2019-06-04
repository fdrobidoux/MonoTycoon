using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.Common
{
    public sealed class TimerTask : IUpdateable
    {
        private readonly Action _performTask;
        private double _intervalMs;
        private bool _recurring;
        private bool _isFinished;
        private double _elapsedMs;

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
        public bool Recurring
        {
            get => _recurring;
            set => _recurring = value;
        }
        public bool IsFinished => _isFinished;
        public double ElapsedMs => _elapsedMs;

        public TimerTask(Action performTask, double intervalMs, bool recurring = true)
        {
            _performTask = performTask;
            _intervalMs = intervalMs;
            _recurring = recurring;
        }

        public void Update(TimeSpan delta)
        {
            if (!Enabled)
                return;

            if (_isFinished)
                return;

            _elapsedMs += delta.TotalMilliseconds;
            if (!(_elapsedMs > _intervalMs))
                return;

            _performTask();
            _isFinished = true;
            if (_recurring)
                Reset();
        }

        public void Update(GameTime gameTime) => Update(gameTime.ElapsedGameTime);

        /// <summary>
        ///     Resets the timer, and sets the timer as unfinished.
        /// </summary>
        /// <param name="modEnabled">A <see langword="bool"/> if it's necessary to change enabled status, or <see langword="null"/> if otherwise.</param>
        public void Reset(bool? modEnabled = null)
        {
            _elapsedMs = 0;
            _isFinished = false;

            if (modEnabled is bool en) 
                Enabled = en;
        }
    }
}