Feature: AnsiMoveCommands
	In order to test the ansi screen writer movement commands
	As a programmer
	I want to send movements and check the final location

Scenario: MoveCursorToLocation
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (0,0)
	When I send '<ESC>[10;40H'
	Then the MockConsole position should be (9,39)

Scenario: MoveCursorToLocationDefaultRow
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<ESC>[;40H'
	Then the MockConsole position should be (0,39)

Scenario: MoveCursorToLocationDefaultColumn
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<ESC>[15H'
	Then the MockConsole position should be (14,0)

Scenario: MoveCursorToLocationDefaultColumnBlankSecond
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<ESC>[15;H'
	Then the MockConsole position should be (14,0)

Scenario: MoveCursorToLocationDefaultColumnRow
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<ESC>[H'
	Then the MockConsole position should be (0,0)

Scenario: MoveCursorToLocationAlternate
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (0,0)
	When I send '<ESC>[23;56f'
	Then the MockConsole position should be (22,55)

Scenario: MoveCursorToLocationDefaultRowAlternate
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<ESC>[;40f'
	Then the MockConsole position should be (0,39)

Scenario: MoveCursorToLocationDefaultColumnAlternate
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<ESC>[15f'
	Then the MockConsole position should be (14,0)

Scenario: MoveCursorToLocationDefaultColumnBlankSecondAlternate
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<ESC>[15;f'
	Then the MockConsole position should be (14,0)

Scenario: MoveCursorToLocationDefaultColumnRowAlternate
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<ESC>[f'
	Then the MockConsole position should be (0,0)

Scenario Outline: MoveCursorUp
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<Command>'
	Then the MockConsole top should be '<Top>'
Examples:
| Command   | Top |
| <ESC>[A   | 9   |
| <ESC>[3A  | 7   |
| <ESC>[9A  | 1   |
| <ESC>[10A | 0   |
| <ESC>[11A | 0   |
| <ESC>[23A | 0   |

Scenario Outline: MoveCursorDown
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<Command>'
	Then the MockConsole top should be '<Top>'
Examples:
| Command   | Top |
| <ESC>[B   | 11  |
| <ESC>[3B  | 13  |
| <ESC>[13B | 23  |
| <ESC>[14B | 24  |
| <ESC>[15B | 24  |
| <ESC>[25B | 24  |

Scenario Outline: MoveCursorForward
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,70)
	When I send '<Command>'
	Then the MockConsole left should be '<Left>'
Examples:
| Command   | Left |
| <ESC>[C   | 71   |
| <ESC>[3C  | 73   |
| <ESC>[8C  | 78   |
| <ESC>[9C  | 79   |
| <ESC>[10C | 79   |
| <ESC>[25C | 79   |

Scenario Outline: MoveCursorBackward
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (10,20)
	When I send '<Command>'
	Then the MockConsole left should be '<Left>'
Examples:
| Command   | Left |
| <ESC>[D   | 19   |
| <ESC>[3D  | 17   |
| <ESC>[19D | 1    |
| <ESC>[20D | 0    |
| <ESC>[21D | 0    |
| <ESC>[25D | 0    |