// <copyright file="ParameterParserTests.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat.Tests
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the parameter parser.
    /// </summary>
    [TestClass]
    public class ParameterParserTests
    {
        private const string Filename = "filename.txt";

        /// <summary>
        /// Checks no arguments gives an error.
        /// </summary>
        [TestMethod]
        public void ParserWithNoArgumentsOutputsUsage()
        {
            var textWriter = new StringWriter();

            var parameters = ParameterParser.Parse(new string[] { }, textWriter);

            Assert.IsNull(parameters);
            var outputString = textWriter.ToString();
            StringAssert.Matches(outputString, new Regex(".*No file name.*Usage.*", RegexOptions.Singleline));
        }

        /// <summary>
        /// Checks two arguments gives an error
        /// </summary>
        [TestMethod]
        public void ParserWithTwoArgumentsOutputsUsageWithError()
        {
            var textWriter = new StringWriter();

            var parameters = ParameterParser.Parse(new string[] { Filename, Filename }, textWriter);

            Assert.IsNull(parameters);
            var outputString = textWriter.ToString();
            StringAssert.Matches(outputString, new Regex(".*Only one.*Usage.*", RegexOptions.Singleline));
        }

        /// <summary>
        /// Checks help shows usage
        /// </summary>
        [TestMethod]
        public void ParserWithHelpOutputsUsageWithNoError()
        {
            var textWriter = new StringWriter();

            var parameters = ParameterParser.Parse(new string[] { "--help" }, textWriter);

            Assert.IsNull(parameters);
            var outputString = textWriter.ToString();
            StringAssert.StartsWith(outputString, "Usage");
        }

        /// <summary>
        /// Checks Bad parameter shows usage
        /// </summary>
        [TestMethod]
        public void ParserWithBadParameterOutputsUsageWithError()
        {
            var textWriter = new StringWriter();

            var parameters = ParameterParser.Parse(new string[] { "--bad" }, textWriter);

            Assert.IsNull(parameters);
            var outputString = textWriter.ToString();
            StringAssert.Matches(outputString, new Regex(".*bad.*Usage.*", RegexOptions.Singleline));
        }

        /// <summary>
        /// Checks a file name on it's own.
        /// </summary>
        [TestMethod]
        public void ParserWithFileNameArgumentsHasCorrectDefaultsAndNoError()
        {
            var textWriter = new StringWriter();

            var parameters = ParameterParser.Parse(new string[] { Filename }, textWriter);

            Assert.IsNotNull(parameters);
            Assert.AreEqual(string.Empty, textWriter.ToString());
            Assert.AreEqual(Filename, parameters.Name);
            Assert.AreEqual(9600, parameters.BaudRate);
            Assert.AreEqual(10416, parameters.TimePerCharacter.Ticks);
            Assert.IsFalse(parameters.CpuFriendly);
        }

        /// <summary>
        /// Checks a file name on it's own.
        /// </summary>
        [TestMethod]
        public void ParserWithAllArgumentsHasCorrectValuesAndNoError()
        {
            var textWriter = new StringWriter();

            var parameters = ParameterParser.Parse(
                new string[]
                {
                    Filename,
                    "--Baudrate",
                    "19200",
                    "--CPUFriendly"
                },
                textWriter);

            Assert.IsNotNull(parameters);
            Assert.AreEqual(string.Empty, textWriter.ToString());
            Assert.AreEqual(Filename, parameters.Name);
            Assert.AreEqual(19200, parameters.BaudRate);
            Assert.AreEqual(5208, parameters.TimePerCharacter.Ticks);
            Assert.IsTrue(parameters.CpuFriendly);
        }
    }
}