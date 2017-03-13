// <copyright file="AnsiScreenWriterDriver.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat.Tests.Drivers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AnsiCat;
    using Moq;

    /// <summary>
    /// Driver class for the Ansi screen writer.
    /// </summary>
    public class AnsiScreenWriterDriver
    {
        private AnsiScreenWriter screenWriter;
        private Mock<IDelayStrategy> mockDelayStrategy;
        private List<ConsoleColor> foregroundColors = new List<ConsoleColor>();
        private StringBuilder writeOutput = new StringBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiScreenWriterDriver"/> class.
        /// </summary>
        public AnsiScreenWriterDriver()
        {
            this.MockConsole = new Mock<IConsole>();
            this.mockDelayStrategy = new Mock<IDelayStrategy>();
            this.screenWriter = new AnsiScreenWriter(this.MockConsole.Object, this.mockDelayStrategy.Object);
            this.MockConsole.SetupSet(p => p.ForegroundColor = It.IsAny<ConsoleColor>()).
                Callback<ConsoleColor>(c => this.ForegroundColors.Add(c));
            this.MockConsole.SetupGet(p => p.ForegroundColor).
                Returns(() => this.ForegroundColors.LastOrDefault());
            this.MockConsole.Setup(m => m.Write(It.IsAny<string>())).
                Callback((string s) => this.WriteOutput.Append(s));
        }

        /// <summary>
        /// Gets or sets the Mock console object.
        /// </summary>
        public Mock<IConsole> MockConsole { get; set; }

        /// <summary>
        /// Gets the output written by the Mock Console.
        /// </summary>
        public StringBuilder WriteOutput
        {
            get
            {
                return this.writeOutput;
            }

            private set
            {
                this.writeOutput = value;
            }
        }

        /// <summary>
        /// Gets the values passed into SetForeground
        /// </summary>
        public List<ConsoleColor> ForegroundColors
        {
            get
            {
                return this.foregroundColors;
            }

            private set
            {
                this.foregroundColors = value;
            }
        }

        /// <summary>
        /// Converts text from a unicode string to codepage 437 then
        /// calls the writer.
        /// </summary>
        /// <param name="bytes">string to convert to bytes.</param>
        public void WriteAnsiScreenWriter(string bytes)
        {
            bytes = bytes.Replace("<ESC>", "\x1b");
            var encoding = Encoding.GetEncoding(437);
            var data = encoding.GetBytes(bytes);
            this.screenWriter.Write(data);
        }
    }
}
