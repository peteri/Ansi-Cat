// <copyright file="Program.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat
{
    using System;

    /// <summary>
    /// The main program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args">Arguments from the command line.</param>
        public static void Main(string[] args)
        {
            try
            {
                var parameters = ParameterParser.Parse(args, Console.Out);
                if (parameters != null)
                {   var delayAction= parameters.CpuFriendly ? ActionDelay.TenMs : ActionDelay.Accurate;
                    var writer = new AnsiScreenWriter(
                        new RealConsole(),
                        new ActionDelay(parameters.TimePerCharacter, delayAction));
                        writer.Write(AnsiReader.Read(parameters.Name));
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error occurred {0}", ex);
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }
    }
}
