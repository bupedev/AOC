using System.CommandLine;
using AOC.Commands;

var rootCommand = new RootCommand("Bupé's solutions to Advent of Code (https://adventofcode.com)");

rootCommand.AddCommand(PingCommand.Build());
rootCommand.AddCommand(SolveCommand.Build());

return await rootCommand.InvokeAsync(args);