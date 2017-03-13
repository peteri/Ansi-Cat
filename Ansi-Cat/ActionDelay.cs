// <copyright file="ActionDelay.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Action delay class.
    /// </summary>
    public class ActionDelay : IDelayStrategy
    {
        private readonly TimeSpan delay;
        private readonly Action delayingAction;
        private TimeSpan target;
        private Stopwatch timer = new Stopwatch();

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDelay"/> class.
        /// </summary>
        /// <param name="delay">Time to delay for each character.</param>
        /// <param name="delayingAction">Action to cause a delay.</param>
        public ActionDelay(TimeSpan delay, Action delayingAction)
        {
            this.timer = new Stopwatch();
            this.timer.Start();
            this.delay = delay;
            this.delayingAction = delayingAction;
            this.target = new TimeSpan(0L);
        }

        /// <summary>
        /// Gets accurate timer action, causes lots of CPU usage as the code just loops.
        /// </summary>
        public static Action Accurate
        {
            get
            {
                return () => { };
            }
        }

        /// <summary>
        /// Gets inaccurate timer action, nicer for the CPU as it sleeps for 10ms.
        /// </summary>
        public static Action TenMs
        {
            get
            {
                return () =>
                {
                    Thread.Sleep(10);
                };
            }
        }

        /// <inheritdoc/>
        public void Delay()
        {
            this.target += this.delay;
            while (this.timer.Elapsed < this.target)
            {
                this.delayingAction();
            }
        }
    }
}
