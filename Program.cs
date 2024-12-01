// See https://aka.ms/new-console-template for more information

using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.Text.Json;
using AOC;

var yearNumberOption = new Option<int>(["--year", "-y"], "The year of the puzzle.");

var dayNumberOption = new Option<int>(["--day", "-d"], "The day of the puzzle.");

var puzzleFileOption = new Option<FileInfo?>(
    ["--file", "-f"],
    "The file containing the input text for the puzzle. Mutually exclusive with --text (-t)."
);

var puzzleTextOption = new Option<string?>(
    ["--text", "-t"],
    "The input text for the puzzle. Mutually exclusive with --file (-f)."
);

var rootCommand = new RootCommand("Bupé's solutions to Advent of Code (https://adventofcode.com)");

rootCommand.AddOption(yearNumberOption);
rootCommand.AddOption(dayNumberOption);
rootCommand.AddOption(puzzleFileOption);
rootCommand.AddOption(puzzleTextOption);

rootCommand.AddValidator(ValidateExactlyOneOf(yearNumberOption));
rootCommand.AddValidator(ValidateExactlyOneOf(dayNumberOption));
rootCommand.AddValidator(ValidateExactlyOneOf(puzzleFileOption, puzzleTextOption));

rootCommand.SetHandler(RunSolution, yearNumberOption, dayNumberOption, puzzleFileOption, puzzleTextOption);

return await rootCommand.InvokeAsync(args);

void RunSolution(int yearNumber, int dayNumber, FileInfo? puzzleFile, string? puzzleText) {
    // Try get the solution by year and day...
    if (!Solution.ByDay.TryGetValue((yearNumber, dayNumber), out var solution)) {
        Console.Error.WriteLine($"Unable to find solution for {dayNumber}.");
        return;
    }

    // Get the text to input for the puzzle from the provided text or file (we trust the validation worked)...
    var inputText = puzzleText ?? File.ReadAllText(puzzleFile!.FullName);

    var stopwatch = new Stopwatch();

    // Iterate over the solutions for each part...
    foreach (var (partNumber, solutionFunction) in solution.GetFunctionsForAllParts()) {
        // Time the execution of the solution...
        stopwatch.Restart();
        var result = solutionFunction(inputText);
        stopwatch.Stop();

        // Write solution result and diagnostics... 
        var elapsedTime = stopwatch.Elapsed;
        var resultJson = JsonSerializer.Serialize(
            result,
            new JsonSerializerOptions {
                WriteIndented = true
            }
        );
        Console.Out.WriteLine(
            $"Solution for part {partNumber} of day {dayNumber} of {yearNumber} found in {elapsedTime}!"
        );
        Console.Out.WriteLine();
        Console.Out.WriteLine(resultJson);
        Console.Out.WriteLine();
    }
}

ValidateSymbolResult<CommandResult> ValidateExactlyOneOf(params Symbol[] symbols) {
    return result => {
        if (result.Children.Count(child => symbols.Contains(child.Symbol)) == 1) return;
        var symbolNames = string.Join(",", symbols.Select(symbol => symbol.Name));
        result.ErrorMessage = $"You must provide exactly one of the following options: {symbolNames}";
    };
}