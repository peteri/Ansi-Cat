Feature: AnsiMiscCommands
	In order to test the ansi screen writer other commands
	As a programmer
	I want to send movements and check the final location

Scenario: ClearCommandIsCalled
	Given I have AnsiScreenWriterDriver
	When I send '<ESC>[2J'
	Then the MockConsole Clear Function is called 'Once' Times

Scenario: ClearCommandIsCalledNever
	Given I have AnsiScreenWriterDriver
	When I send '<ESC>[0J'
	Then the MockConsole Clear Function is called 'Never' Times

Scenario: ClearCommandIsCalledNeverDefault
	Given I have AnsiScreenWriterDriver
	When I send '<ESC>[J'
	Then the MockConsole Clear Function is called 'Never' Times

Scenario: ClearEolCommandIsCalled
	Given I have AnsiScreenWriterDriver
	When I send '<ESC>[0K'
	Then the MockConsole ClearEol Function is called 'Once' Times

Scenario: ClearEolCommandIsCalledDefault
	Given I have AnsiScreenWriterDriver
	When I send '<ESC>[K'
	Then the MockConsole ClearEol Function is called 'Once' Times

Scenario: ClearEolCommandIsCalledNeverDefault
	Given I have AnsiScreenWriterDriver
	When I send '<ESC>[2K'
	Then the MockConsole ClearEol Function is called 'Never' Times

Scenario: TestSaveAndRestoreScreenLocation
	Given I have AnsiScreenWriterDriver
	And MockConsole position is (5,20)
	When I send '<ESC>[5B<ESC>[20C<ESC>[s<ESC>[10;40H<ESC>[u'
	Then the MockConsole position should be (9,39)
	Then the MockConsole position should be (5,20)
