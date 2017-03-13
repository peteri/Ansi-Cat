// <copyright file="IDelayStrategy.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat
{
    /// <summary>
    /// Interface for inter character delayer.
    /// </summary>
    public interface IDelayStrategy
    {
        /// <summary>
        /// Delays for an inter character time period.
        /// </summary>
        void Delay();
    }
}
