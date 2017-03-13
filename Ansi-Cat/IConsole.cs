// <copyright file="IConsole.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat
{
    using System;

    /// <summary>
    /// Interface to the console, used so we can test properly.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Gets or sets the cursors left position.
        /// </summary>
        int CursorLeft { get; set; }

        /// <summary>
        /// Gets or sets the cursors top position.
        /// </summary>
        int CursorTop { get; set; }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the Cursors background color.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Sets buffer settings back after we've set them to 80x25.
        /// </summary>
        void RestoreBufferSettings();

        /// <summary>
        /// Setx the .net console window to 80x25.
        /// </summary>
        void Set80x25();

        /// <summary>
        /// Sets the current cursor position zero-based.
        /// </summary>
        /// <param name="left">New cursor left position.</param>
        /// <param name="top">New cursor top position.</param>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Writes the string to screen.
        /// </summary>
        /// <param name="s">String to write.</param>
        void Write(string s);

        /// <summary>
        /// Clears the screen.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears to end of line.
        /// </summary>
        void ClearEol();
    }
}
