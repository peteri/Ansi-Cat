// <copyright file="AnsiScreenWriter.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Writes out via an instance of IConsole ansi control codes using codepage 437.
    /// </summary>
    public class AnsiScreenWriter
    {
        private readonly IConsole console;
        private readonly IDelayStrategy delayer;
        private AnsiState ansiState = AnsiState.Normal;
        private List<int?> commandParameters = new List<int?> { null };
        private bool bold = false;
        private int saveX;
        private int saveY;
        private Dictionary<byte, Action<IConsole, List<int?>>> commands;
        private Dictionary<int, Action<IConsole>> colorCommands;
        private Dictionary<AnsiState, Func<IConsole, byte, AnsiState>> stateMachine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiScreenWriter"/> class.
        /// </summary>
        /// <param name="console">Console to use.</param>
        /// <param name="delayer">Inter character delay routine.</param>
        public AnsiScreenWriter(IConsole console, IDelayStrategy delayer)
        {
            this.console = console;
            this.delayer = delayer;
            this.FillInCommandDictionary();
            this.FillInColorCommandDictionary();
            this.FillInStateMachineDictionary();
        }

        private enum AnsiState
        {
            Normal,
            SeenEsc,
            ParseParam
        }

        /// <summary>
        /// Writes an array of bytes using a codepage 437 encoding,
        /// parsing and acting on ANSI escape sequences.
        /// </summary>
        /// <param name="data">Array of bytes to display.</param>
        public void Write(byte[] data)
        {
            this.console.Set80x25();
            foreach (var b in data)
            {
                this.ansiState = this.stateMachine[this.ansiState](this.console, b);
                this.delayer.Delay();
            }

            this.console.RestoreBufferSettings();
            this.console.Write(Environment.NewLine);
        }

        private void FillInStateMachineDictionary()
        {
            this.stateMachine = new Dictionary<AnsiState, Func<IConsole, byte, AnsiState>>
            {
                { AnsiState.Normal, (c, b) => this.Normal(c, b) },
                { AnsiState.SeenEsc, (c, b) => this.SeenEsc(c, b) },
                { AnsiState.ParseParam, (c, b) => this.ParseParam(c, b) }
            };
        }

        private void FillInCommandDictionary()
        {
            this.commands = new Dictionary<byte, Action<IConsole, List<int?>>>
            {
                { (byte)'f', (c, l) => c.SetCursorPosition(l.Count > 1 ? (l[1] ?? 1) - 1 : 0, (l[0] ?? 1) - 1) },
                { (byte)'H', (c, l) => c.SetCursorPosition(l.Count > 1 ? (l[1] ?? 1) - 1 : 0, (l[0] ?? 1) - 1) },
                { (byte)'A', (c, l) => { c.CursorTop = Math.Max(0, Math.Min(c.CursorTop - (l[0] ?? 1), 24)); } },
                { (byte)'B', (c, l) => { c.CursorTop = Math.Max(0, Math.Min(c.CursorTop + (l[0] ?? 1), 24)); } },
                { (byte)'C', (c, l) => { c.CursorLeft = Math.Max(0, Math.Min(c.CursorLeft + (l[0] ?? 1), 79)); } },
                { (byte)'D', (c, l) => { c.CursorLeft = Math.Max(0, Math.Min(c.CursorLeft - (l[0] ?? 1), 79)); } },
                {
                    (byte)'J', (c, l) =>
                    {
                        if ((l[0] ?? 0) == 2)
                        {
                            c.Clear();
                        }
                    }
                },
                {
                    (byte)'K', (c, l) =>
                    {
                        if ((l[0] ?? 0) == 0)
                        {
                            c.ClearEol();
                        }
                    }
                },
                {
                    (byte)'m', (c, l) =>
                    {
                        foreach (var i in l)
                        {
                            if (this.colorCommands.ContainsKey(i ?? 0))
                            {
                                this.colorCommands[i ?? 0](c);
                            }
                        }
                    }
                },
                {
                    (byte)'s', (c, l) =>
                    {
                        this.saveX = c.CursorLeft;
                        this.saveY = c.CursorTop;
                    }
                },
                { (byte)'u', (c, l) => c.SetCursorPosition(this.saveX, this.saveY) }
            };
        }

        private void FillInColorCommandDictionary()
        {
            this.colorCommands = new Dictionary<int, Action<IConsole>>
            {
                {
                    0, (c) =>
                    {
                    this.bold = false;
                    c.ForegroundColor = ConsoleColor.Gray;
                    c.BackgroundColor = ConsoleColor.Black;
                    }
                },
                {
                    1, (c) =>
                    {
                        this.bold = true;
                        if (c.ForegroundColor < ConsoleColor.DarkGray)
                        {
                            c.ForegroundColor += 8;
                        }
                    }
                },
                { 5, (c) => { } },   // would be "blink"
                { 30, (c) => c.ForegroundColor = ConsoleColor.Black + (this.bold ? 8 : 0) },
                { 31, (c) => c.ForegroundColor = ConsoleColor.DarkRed + (this.bold ? 8 : 0) },
                { 32, (c) => c.ForegroundColor = ConsoleColor.DarkGreen + (this.bold ? 8 : 0) },
                { 33, (c) => c.ForegroundColor = ConsoleColor.DarkYellow + (this.bold ? 8 : 0) },
                { 34, (c) => c.ForegroundColor = ConsoleColor.DarkBlue + (this.bold ? 8 : 0) },
                { 35, (c) => c.ForegroundColor = ConsoleColor.DarkMagenta + (this.bold ? 8 : 0) },
                { 36, (c) => c.ForegroundColor = ConsoleColor.DarkCyan + (this.bold ? 8 : 0) },
                { 37, (c) => c.ForegroundColor = ConsoleColor.Gray + (this.bold ? 8 : 0) },
                {
                    39, (c) =>
                    {
                        this.bold = false;
                        c.ForegroundColor = ConsoleColor.Gray;
                    }
                },
                { 40, (c) => c.BackgroundColor = ConsoleColor.Black },
                { 41, (c) => c.BackgroundColor = ConsoleColor.DarkRed },
                { 42, (c) => c.BackgroundColor = ConsoleColor.DarkGreen },
                { 43, (c) => c.BackgroundColor = ConsoleColor.DarkYellow },
                { 44, (c) => c.BackgroundColor = ConsoleColor.DarkBlue },
                { 45, (c) => c.BackgroundColor = ConsoleColor.DarkMagenta },
                { 46, (c) => c.BackgroundColor = ConsoleColor.DarkCyan },
                { 47, (c) => c.BackgroundColor = ConsoleColor.Gray },
                { 49, (c) => c.BackgroundColor = ConsoleColor.Black }
            };
        }

        private AnsiState Normal(IConsole con, byte c)
        {
            if (c == 27)
            {
                return AnsiState.SeenEsc;
            }

            con.Write(Encoding.GetEncoding(437).GetString(new byte[] { c }));
            return AnsiState.Normal;
        }

        private AnsiState SeenEsc(IConsole con, byte c)
        {
            if (c == (byte)'[')
            {
                this.commandParameters = new List<int?> { null };
                return AnsiState.ParseParam;
            }

            return this.Normal(con, c);
        }

        private AnsiState ParseParam(IConsole console, byte c)
        {
            if (c == (byte)';')
            {
                this.commandParameters.Add(null);
                return AnsiState.ParseParam;
            }

            if (char.IsDigit((char)c))
            {
                this.commandParameters[this.commandParameters.Count - 1] =
                    (this.commandParameters[this.commandParameters.Count - 1].GetValueOrDefault() * 10) + (c - 48);
                return AnsiState.ParseParam;
            }

            if (this.commands.ContainsKey(c))
            {
                this.commands[c](console, this.commandParameters);
            }

            return AnsiState.Normal;
        }
    }
}
