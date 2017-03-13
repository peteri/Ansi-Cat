// <copyright file="RealConsole.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat
{
    using System;

    /// <summary>
    /// Wrapper class around the .net console class so we can test.
    /// </summary>
    public class RealConsole : IConsole
    {
        private int savedBufferWidth;
        private int savedBufferHeight;

        /// <inheritdoc/>
        public ConsoleColor BackgroundColor
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        /// <inheritdoc/>
        public int CursorLeft
        {
            get { return Console.CursorLeft; }
            set { Console.CursorLeft = value; }
        }

        /// <inheritdoc/>
        public int CursorTop
        {
            get { return Console.CursorTop; }
            set { Console.CursorTop = value; }
        }

        /// <inheritdoc/>
        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }

        /// <inheritdoc/>
        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        /// <inheritdoc/>
        public void Write(string s)
        {
            Console.Write(s);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc/>
        public void RestoreBufferSettings()
        {
            if (Console.BufferWidth < this.savedBufferWidth)
            {
                Console.BufferWidth = this.savedBufferWidth;
            }

            if (Console.BufferHeight < this.savedBufferHeight)
            {
                Console.BufferHeight = this.savedBufferHeight;
            }

            Console.CursorVisible = true;
        }

        /// <inheritdoc/>
        public void Set80x25()
        {
            Console.SetWindowPosition(0, 0);
            this.savedBufferHeight = Console.BufferHeight;
            this.savedBufferWidth = Console.BufferWidth;
            if (Console.BufferWidth <= 80)
            {
                Console.BufferWidth = 80;
            }

            if (Console.BufferHeight <= 25)
            {
                Console.BufferHeight = 25;
            }

            Console.SetWindowSize(80, 25);
            Console.SetBufferSize(80, 25);
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <inheritdoc/>
        public void ClearEol()
        {
            int currentLineCursor = Console.CursorTop;
            int currentCursorLeft = Console.CursorLeft;
            Console.Write(new string(' ', Console.WindowWidth - currentCursorLeft));
            Console.SetCursorPosition(currentCursorLeft, currentLineCursor);
        }
    }
}
