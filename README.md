# Advent of Code (AoC)

In 2023, I attempted Advent of Code in F#. It was my first experience with a functional programming language and was
reasonably enjoyable, but I found that learning a new language while participating in Advent of Code was a bit too much.
In 2024, that sentiment is resonating even more strongly as I have two small children at home now. I've decided that I
would like to host my Advent of Code solutions for all years here in the language I use the most (C#).

## Running Solutions

I looked into some previous years of Advent of Code and I noticed that most puzzles require you to write an algorithm
that processes some randomized text. To accomodate this, I have established an C# console application project with
command-line arguments:

```text
Description:
  Bupé's solutions to Advent of Code (https://adventofcode.com)

Usage:
  AOC [options]

Options:
  -y, --year <year>  The year of the puzzle.
  -d, --day <day>    The day of the puzzle.
  -f, --file <file>  The file containing the input text for the puzzle. Mutually exclusive with --text (-t).
  -t, --text <text>  The input text for the puzzle. Mutually exclusive with --file (-f).
  --version          Show version information
  -?, -h, --help     Show help and usage information
```

The entry point for the application uses `System.Reflection` (I know, I am definitely on Santa's naughty list this
year), to select the solution code for each day and year. Each solution is an extension of the `Solution` class which
must provide some key information about the solution (day, year and some functions that return the result of the
solution given some input text).

```csharp
public class DayXXYearYYYY() : Solution(yyyy, xx) {
    protected override object GetPart1Result(string input) {
        return /*solution to part 1*/;
    }

    protected override object GetPart2Result(string input) {
        return /*solution to part 2*/;
    }
}
```

As a test, I have included `D00Example.cs` in the project, which will print information about the input back to the
console:

```text
> .\AOC.exe -y 0 -d 0 -t "Hello World!"
Solution for part 1 of day 0 of 0 found in 00:00:00.0003227!

{
  "Lines": [
    "Hello world!"
  ]
}

Solution for part 2 of day 0 of 0 found in 00:00:00.0004838!

{
  "CharCount": 12
}
```