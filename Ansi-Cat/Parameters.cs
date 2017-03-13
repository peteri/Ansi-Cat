// <copyright file="Parameters.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat
{
    using System;

    /// <summary>
    /// Holds the command line parameters once parsed.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether the system should use Thread.Sleep.
        /// </summary>
        public bool CpuFriendly { get; set; }

        /// <summary>
        /// Gets or sets the name to use for a url or filename on disk.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the simulated baud rate.
        /// </summary>
        public int BaudRate { get; set; } = 9600;

        /// <summary>
        /// Gets the amount of time for each character.
        /// </summary>
        public TimeSpan TimePerCharacter
        {
            get
            {
                // Assume 8-N-1 for 10 bits per character....
                return TimeSpan.FromTicks(TimeSpan.TicksPerSecond * 10L / this.BaudRate);
            }
        }
    }
}
