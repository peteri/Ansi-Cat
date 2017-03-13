# Ansi-Cat - Plays back .ans files on Windows.

[![Build status](https://ci.appveyor.com/api/projects/status/746fflggti7sywxf?svg=true)](https://ci.appveyor.com/project/peteri/ansi-cat)
[![codecov.io](http://codecov.io/github/peteri/Ansi-Cat/coverage.svg?branch=master)](http://codecov.io/github/peteri/Ansi-Cat?branch=master)

## Description
Inspired by a [question](http://retrocomputing.stackexchange.com/questions/2424/is-there-a-simple-way-to-display-ansi-art-and-animation-files-in-a-modern-termin) on the retrocomputing stackexchange site, Ansi-Cat is a utility to play back .ans files (ansi.sys) on modern Windows platform from the command line. 

### Background
".ans" files are instructions to ansi terminals to move cursors, change colors and display pretty pictures. Popular with BBS operators in the 80s/90s they need ansi.sys or a terminal emulator to display correctly. Windows command prompts don't normally support ansi.sys, have the older PC 437 codepage active or allow you to play back the file at the correct speed. Ansi-Cat was written to fill this gap.

## Installation
Grab the latest release from [here](https://www.github.com/peteri/Ansi-Cat/releases/latest)

If you have the .net framework 4.5.2 already installed you're good to go. Just unzip and follow the instructions below on how to run.

The web installer for .net framework 4.5.2 should be [here](https://www.microsoft.com/en-gb/download/details.aspx?id=42643)

## Usage
In a windows cmd.exe prompt window
```
Ansi-Cat fighters.ans
Ansi-Cat fighters.ans --baudrate 14400
Ansi-Cat http://www.example.com/ansiart/file.ans --baudrate 9600
``` 
*Warning: Ansi-Cat will reset the command window to have a width and height of 80x25 to emulate a 90s PC screen, it will only restore the scroll back height on exit (not the window). It also turns off the cursor to stop flickering, currently if you interrupt playback the command window might not display a cursor.*

### Parameters
`--baudrate value` Sets the playback baud rate, defaults to 9600 if omitted, depending on the age of the file any value from 300-19200 would be sensible.

`--CpuFriendly` Ansi-Cat has two mechanisms for delaying, by default a simple delay loop is used, however an alternative where the process is put to sleep can be used (and it seems to give reasonable results) which is a bit more CPU friendly (but can give slightly lumpier animation)