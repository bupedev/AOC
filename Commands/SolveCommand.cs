using System.CommandLine;
using System.Diagnostics;
using System.Text.Json;

namespace AOC.Commands;

public static class SolveCommand {
    private static readonly Option<int> YearNumberOption = new(
        ["--year", "-y"],
        "The year of the puzzle."
    );

    private static readonly Option<int> DayNumberOption = new(
        ["--day", "-d"],
        "The day of the puzzle."
    );

    private static readonly Option<FileInfo?> PuzzleFileOption = new(
        ["--file", "-f"],
        "The file containing the input text for the puzzle. Mutually exclusive with --text (-t)."
    );

    private static readonly Option<string?> PuzzleTextOption = new(
        ["--text", "-t"],
        "The input text for the puzzle. Mutually exclusive with --file (-f)."
    );

    public static Command Build() {
        var command = new Command("solve", "Solve the puzzle on a particular year and day.");

        command.AddOption(YearNumberOption);
        command.AddOption(DayNumberOption);
        command.AddOption(PuzzleFileOption);
        command.AddOption(PuzzleTextOption);

        command.AddValidator(Validations.ExactlyOneOf(YearNumberOption));
        command.AddValidator(Validations.ExactlyOneOf(DayNumberOption));
        command.AddValidator(Validations.NoMoreThanOneOf(PuzzleFileOption, PuzzleTextOption));

        command.SetHandler(Solve, YearNumberOption, DayNumberOption, PuzzleFileOption, PuzzleTextOption);

        return command;
    }

    private static void Solve(int yearNumber, int dayNumber, FileInfo? puzzleFile, string? puzzleText) {
        // Try get the solution by year and day...
        if (!Solution.ByYearAndDay.TryGetValue((yearNumber, dayNumber), out var solution)) {
            Console.Error.WriteLine($"Unable to find solution for {dayNumber}.");
            return;
        }

        var labeledInputText = new List<(string Label, string Input)>();

        // Get the inputs for the puzzle (both from CLI and service)...
        if (puzzleText is not null) labeledInputText.Add(("Sample", puzzleText));
        if (puzzleFile is not null) labeledInputText.Add(("Sample", File.ReadAllText(puzzleFile.FullName)));
        if (CacheService.TryGetPuzzleInput(yearNumber, dayNumber, out var finalInput))
            labeledInputText.Add(("Final", finalInput));

        if (!labeledInputText.Any()) {
            Console.Error.WriteLine("No puzzle inputs found!");
            return;
        }

        var stopwatch = new Stopwatch();

        // Iterate over the inputs and solutions for each part...
        foreach (var (label, input) in labeledInputText)
        foreach (var (partNumber, solutionFunction) in solution.GetFunctionsForAllParts()) {
            // Time the execution of the solution...
            stopwatch.Restart();
            var result = solutionFunction(input);
            stopwatch.Stop();

            // Write solution result and diagnostics... 
            var elapsedTime = stopwatch.Elapsed;
            var resultText = result as string ??
                             JsonSerializer.Serialize(
                                 result,
                                 new JsonSerializerOptions {
                                     WriteIndented = true
                                 }
                             );

            Console.Out.WriteLine(
                $"{label} solution for part {partNumber} of day {dayNumber} of {yearNumber} found in {elapsedTime}!"
            );
            Console.Out.WriteLine();
            Console.Out.WriteLine(resultText);
            Console.Out.WriteLine();
        }
    }
}