Feature: AnsiToConsoleColorTranslation
	In order to check the color codes
	As a programmer
	I want to be able to send ANSI strings and have the console change color

Scenario Outline: Test background colors
	Given I have AnsiScreenWriterDriver
	When I send '<Command>'
	Then the background console color should be '<Color>'
Examples:
| Command   | Color       |
| <ESC>[40m | Black       |
| <ESC>[41m | DarkRed     |
| <ESC>[42m | DarkGreen   |
| <ESC>[43m | DarkYellow  |
| <ESC>[44m | DarkBlue    |
| <ESC>[45m | DarkMagenta |
| <ESC>[46m | DarkCyan    |
| <ESC>[47m | Gray        |
| <ESC>[49m | Black       |

Scenario Outline: Test foreground colors
	Given I have AnsiScreenWriterDriver
	When I send '<Command>'
	Then the foreground console color should be '<Color>' 
Examples:
| Command   | Color       |
| <ESC>[30m | Black       |
| <ESC>[31m | DarkRed     |
| <ESC>[32m | DarkGreen   |
| <ESC>[33m | DarkYellow  |
| <ESC>[34m | DarkBlue    |
| <ESC>[35m | DarkMagenta |
| <ESC>[36m | DarkCyan    |
| <ESC>[37m | Gray        |
| <ESC>[39m | Gray        |

Scenario Outline: Test foreground colors with bold
	Given I have AnsiScreenWriterDriver
	When I send '<Command>'
	Then the foreground console color should be set to '<ColorFirst>' Followed By'<Color1>' Finally '<FinalColor>'.
Examples:
| Command                   | ColorFirst | Color1 | FinalColor |
| <ESC>[0m<ESC>[1;30m       | Gray       | White  | DarkGray   |
| <ESC>[0m<ESC>[1;31m       | Gray       | White  | Red        |
| <ESC>[0m<ESC>[1;32m       | Gray       | White  | Green      |
| <ESC>[0m<ESC>[1;33m       | Gray       | White  | Yellow     |
| <ESC>[0m<ESC>[1;34m       | Gray       | White  | Blue       |
| <ESC>[0m<ESC>[1;35m       | Gray       | White  | Magenta    |
| <ESC>[0m<ESC>[1;36m       | Gray       | White  | Cyan       |
| <ESC>[0m<ESC>[1;37m       | Gray       | White  | White      |
| <ESC>[32m<ESC>[1;39m      | DarkGreen  | Green  | Gray       |
| <ESC>[0m<ESC>[39;1m       | Gray       | Gray   | White      |
| <ESC>[33m<ESC>[1;37m      | DarkYellow | Yellow | White      |
| <ESC>[1m<ESC>[33;0m       | DarkGray   | Yellow | Gray       |
| <ESC>[33m<ESC>[5;37m      | DarkYellow | Skip   | Gray       |
| <ESC>[31m<ESC>[38;37m     | DarkRed    | Skip   | Gray       |
| <ESC>[31m<ESC>[m<ESC>[37m | DarkRed    | Gray   | Gray       |
