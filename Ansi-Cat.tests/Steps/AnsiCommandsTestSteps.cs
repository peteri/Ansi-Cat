// <copyright file="AnsiCommandsTestSteps.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat.Tests.Steps
{
    using System;
    using Drivers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Test steps for the screen writer.
    /// </summary>
    [Binding]
    public class AnsiCommandsTestSteps
    {
        private readonly AnsiScreenWriterDriver ansiWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiCommandsTestSteps"/> class.
        /// </summary>
        /// <param name="ansiWriter">Instance of the screen writer driver.</param>
        public AnsiCommandsTestSteps(AnsiScreenWriterDriver ansiWriter)
        {
            this.ansiWriter = ansiWriter;
        }

        /// <summary>
        /// Do nothing operation since we get this as part of the constructor.
        /// </summary>
        [Given(@"I have AnsiScreenWriterDriver")]
        public void GivenIHaveAnsiScreenWriterDriver()
        {
        }

        /// <summary>
        /// Sets the console position and moq responses for cursor left and top properties.
        /// </summary>
        /// <param name="row">Row.</param>
        /// <param name="colummn">Column.</param>
        [Given(@"MockConsole position is \((\d*),(\d*)\)")]
        public void GivenMockConsolePositionIs(int row, int colummn)
        {
            this.ansiWriter.MockConsole.Setup(p => p.CursorLeft).Returns(colummn);
            this.ansiWriter.MockConsole.Setup(p => p.CursorTop).Returns(row);
        }

        /// <summary>
        /// Sends a command string to the ansi writer.
        /// </summary>
        /// <param name="ansiCommands">Commands to send as a string.</param>
        [When(@"I send '(.*)'")]
        public void WhenISend(string ansiCommands)
        {
            this.ansiWriter.WriteAnsiScreenWriter(ansiCommands);
        }

        /// <summary>
        /// Checks the current cursor position.
        /// </summary>
        /// <param name="row">Row.</param>
        /// <param name="column">Column.</param>
        [Then(@"the MockConsole position should be \((\d*),(\d*)\)")]
        public void ThenTheMockConsolePositionShouldBe(int row, int column)
        {
            this.ansiWriter.MockConsole.Verify(
                m => m.SetCursorPosition(
                It.Is<int>(left => left == column),
                It.Is<int>(top => top == row)),
                Times.Once);
        }

        /// <summary>
        /// Checks the console left position.
        /// </summary>
        /// <param name="column">Column.</param>
        [Then(@"the MockConsole left should be '(\d*)'")]
        public void ThenTheMockConsoleLeftShouldBe(int column)
        {
            this.ansiWriter.MockConsole.VerifySet(p => p.CursorLeft = column, Times.Once);
        }

        /// <summary>
        /// Checks the console top position.
        /// </summary>
        /// <param name="row">Row.</param>
        [Then(@"the MockConsole top should be '(\d*)'")]
        public void ThenTheMockConsoleTopShouldBe(int row)
        {
            this.ansiWriter.MockConsole.VerifySet(p => p.CursorTop = row, Times.Once);
        }

        /// <summary>
        /// Checks that a color was assigned to the back ground.
        /// </summary>
        /// <param name="colorName">Color to check for.</param>
        [Then(@"the background console color should be '(.*)'")]
        public void ThenTheBackgroundConsoleColorShouldBe(string colorName)
        {
            var consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorName);
            this.ansiWriter.MockConsole.VerifySet(p => p.BackgroundColor = consoleColor, Times.Once);
        }

        /// <summary>
        /// Checks that a color was assigned to fore ground.
        /// </summary>
        /// <param name="colorName">Color to check for.</param>
        [Then(@"the foreground console color should be '(.*)'")]
        public void ThenTheForegroundConsoleColorShouldBe(string colorName)
        {
            var consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorName);
            this.ansiWriter.MockConsole.VerifySet(p => p.ForegroundColor = consoleColor, Times.Once);
        }

        /// <summary>
        /// Multi stage assignemnt checker for colors.
        /// </summary>
        /// <param name="firstColorName">First color assigned.</param>
        /// <param name="middleColorName">Middle color assigned.</param>
        /// <param name="finalColorName">Final color assigned.</param>
        [Then(@"the foreground console color should be set to '(.*)' Followed By'(.*)' Finally '(.*)'\.")]
        public void ThenTheForegroundConsoleColorShouldBeSetToFollowedByFinally_(string firstColorName, string middleColorName, string finalColorName)
        {
            var firstColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), firstColorName);
            var finalColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), finalColorName);
            if (middleColorName != "Skip")
            {
                var middleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), middleColorName);
                Assert.AreEqual(3, this.ansiWriter.ForegroundColors.Count);
                Assert.AreEqual(firstColor, this.ansiWriter.ForegroundColors[0], "First");
                Assert.AreEqual(middleColor, this.ansiWriter.ForegroundColors[1], "Second");
                Assert.AreEqual(finalColor, this.ansiWriter.ForegroundColors[2], "Final");
            }
            else
            {
                Assert.AreEqual(2, this.ansiWriter.ForegroundColors.Count);
                Assert.AreEqual(firstColor, this.ansiWriter.ForegroundColors[0], "First");
                Assert.AreEqual(finalColor, this.ansiWriter.ForegroundColors[1], "Final");
            }
        }

        /// <summary>
        /// Checks if the mock console clear function was called.
        /// </summary>
        /// <param name="times">String of either Never or Once.</param>
        [Then(@"the MockConsole Clear Function is called '(.*)' Times")]
        public void ThenTheMockConsoleClearFunctionIsCalledTimes(string times)
        {
            this.ansiWriter.MockConsole.Verify(
                m => m.Clear(),
                (times == "Never") ? Times.Never() : Times.Once());
        }

        /// <summary>
        /// Checks if the mock console clear to end of line function was called.
        /// </summary>
        /// <param name="times">String of either Never or Once.</param>
        [Then(@"the MockConsole ClearEol Function is called '(.*)' Times")]
        public void ThenTheMockConsoleClearEolFunctionIsCalledTimes(string times)
        {
            this.ansiWriter.MockConsole.Verify(
                m => m.ClearEol(),
                (times == "Never") ? Times.Never() : Times.Once());
        }

        /// <summary>
        /// Checks what string values were written to the mock console.
        /// </summary>
        /// <param name="expected">Expected string to write.</param>
        [Then(@"the MockConsole should write '(.*)'")]
        public void ThenTheMockConsoleShouldWrite(string expected)
        {
            var actualOutput = this.ansiWriter.WriteOutput.ToString();
            Assert.AreEqual(expected + Environment.NewLine, actualOutput);
        }
    }
}
