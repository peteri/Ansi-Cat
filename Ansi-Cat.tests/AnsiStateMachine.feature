Feature: AnsiStateMachine
	In order to check the ansi state machine
	As a programmer
	I want to check the output is correct

Scenario: Single escape works
	Given I have AnsiScreenWriterDriver
	When I send '<ESC>Hello'
	Then the MockConsole should write 'Hello'

Scenario: Multiple escapes find correctly
	Given I have AnsiScreenWriterDriver
	When I send '<ESC>O<ESC><ESC>[XK'
	Then the MockConsole should write 'OK'
