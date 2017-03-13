// <copyright file="ParameterParser.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat
{
    using System;
    using System.IO;

    /// <summary>
    /// Parameter parsing class.
    /// </summary>
    public class ParameterParser
    {
        /// <summary>
        /// Parses command line arguments.
        /// </summary>
        /// <param name="args">Array of arguments from the command line.</param>
        /// <param name="output">Stream to write usages out to.</param>
        /// <returns>Filled in parameter class.</returns>
        public static Parameters Parse(string[] args, TextWriter output)
        {
            var result = new Parameters();
            int p = 0;
            while (p < args.Length)
            {
                if (args[p].StartsWith("--"))
                {
                    var option = args[p].ToLower();
                    switch (option)
                    {
                        case "--cpufriendly": result.CpuFriendly = true; break;
                        case "--baudrate": result.BaudRate = int.Parse(args[++p]); break;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(result.Name))
                    {
                        Usage(output);
                        throw new ArgumentException("Only one file name or url allowed.");
                    }

                    result.Name = args[p];
                }

                p++;
            }

            if (string.IsNullOrEmpty(result.Name))
            {
                Usage(output);
                throw new ArgumentException("No file name or url provided.");
            }

            return result;
        }

        private static void Usage(TextWriter output)
        {
            output.WriteLine("Usage ansi-cat name [--baudrate 57600] [--cpufriendly]");
            output.WriteLine("Where: name is either a url or filename.");
            output.WriteLine("       --baudrate rate - simulates a particular baud rate.");
            output.WriteLine("       --cpufriendly   - use thread.sleep for baud rate throttling. ");
        }
    }
}
