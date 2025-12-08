using System.CommandLine;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AOC.Commands;

public static class LeaderboardCommand
{
    private static readonly Option<int> YearNumberOption = new(
        ["--year", "-y"],
        "The year of the leaderboard"
    );

    private static readonly Option<int> LeaderboardIdOption = new(
        ["--id", "-i"],
        "The ID of the leaderboard (you can find this in the URL of the leaderboard page)"
    );

    private static readonly Option<int[]> DaysOption = new(
        ["--days", "-d"],
        () => [],
        "The days for which to display the leaderboard. If not specified, all days will be displayed."
    )
    {
        AllowMultipleArgumentsPerToken = true
    };

    private static readonly Option<bool> ShowPart2DiffOption = new(
        ["--part2diff", "-p"],
        "If true, only the time taken to solve part 2 after part 1 will be displayed in the leaderboard."
    );

    public static Command Build()
    {
        var command = new Command(
            "leaderboard",
            "Request a leaderboard from the server and print it to the console."
        );

        command.AddOption(YearNumberOption);
        command.AddOption(LeaderboardIdOption);
        command.AddOption(DaysOption);
        command.AddOption(ShowPart2DiffOption);

        command.AddValidator(Validations.ExactlyOneOf(YearNumberOption));
        command.AddValidator(Validations.ExactlyOneOf(LeaderboardIdOption));

        command.SetHandler(Handle, YearNumberOption, LeaderboardIdOption, DaysOption, ShowPart2DiffOption);

        return command;
    }

    private static void Handle(int year, int leaderboardId, int[] days, bool showPart2Diff)
    {
        if (!CacheService.TryGetLeaderboard(year, leaderboardId, out var json))
        {
            Console.Error.WriteLine("Failed to retrieve leaderboard! Please ensure that the leaderboard ID is " +
                                    "correct and that it has been less than 15 minutes since the last leaderboard " +
                                    "request to the server.");
        }

        Print(json, days, showPart2Diff);
    }

    private static void Print(string json, int[] days, bool showPart2Diff)
    {
        var leaderboard = JsonSerializer.Deserialize<Leaderboard>(json, SerializerOptions);

        if (leaderboard?.Members is null)
        {
            Console.WriteLine("Cannot display leaderboard: malformed data!");

            return;
        }

        var rows = BuildTableRows(leaderboard, days, showPart2Diff);
        var widths = CalculateColumnWidths(rows);

        WriteTable(rows, widths);
    }

    private static void WriteTable(string[][] rows, int[] widths)
    {
        for (var r = 0; r < rows.Length; r++)
        {
            // Write each row...
            var firstRow = r is 0;
            var row = rows[r];
            Console.WriteLine(string.Join(" | ", widths.Index().Select(width => firstRow || width.Index == 0
                ? row[width.Index].PadRight(width.Item)
                : row[width.Index].PadLeft(width.Item))));

            // Write the header divider after the first row...
            if (firstRow)
            {
                Console.WriteLine(string.Join("-+-", widths.Select(w => new string('-', w))));
            }
        }
    }

    private static int[] CalculateColumnWidths(string[][] rows)
    {
        return Enumerable
            .Range(0, rows[0].Length)
            .Select(c => rows.Select(r => r[c].Length).Prepend(0).Max())
            .ToArray();
    }

    private static string[][] BuildTableRows(Leaderboard leaderboard, int[] days, bool showPart2Diff)
    {
        if (days.Length == 0)
        {
            var recordedDays = leaderboard.Members.Values.Max(member =>
            {
                var resultDays = member.Results.Keys;
                return resultDays.Count != 0 ? member.Results.Keys.Max() : 0;
            });

            days = Enumerable.Range(1, Math.Min(leaderboard.Days, recordedDays)).ToArray();
        }

        return
        [
            [
                "Name",
                "Stars",
                "Score",
                ..days.SelectMany<int, string>(day =>
                    showPart2Diff ? [$"D{day:00}"] : [$"D{day:00} P1", $"D{day:00} P2"])
            ],
            ..leaderboard.Members.Values
                .Where(member => member.Score > 0)
                .OrderByDescending(m => m.Score)
                .Select<Member, string[]>(member =>
                [
                    member.Name,
                    member.Stars.ToString(),
                    member.Score.ToString(),
                    ..days.SelectMany<int, string>(day =>
                    {
                        if (!member.Results.TryGetValue(day, out var result))
                        {
                            return [string.Empty, string.Empty];
                        }

                        return showPart2Diff
                            ? [GetFormattedPart2DiffTime(result)]
                            :
                            [
                                GetFormattedTime(result, leaderboard, day, 1),
                                GetFormattedTime(result, leaderboard, day, 2)
                            ];
                    })
                ])
        ];
    }

    private static string GetFormattedTime(
        Dictionary<int, Result> resultsByPart,
        Leaderboard leaderboard,
        int day,
        int part)
    {
        if (resultsByPart.TryGetValue(part, out var result))
        {
            var resultTimestamp = DateTimeOffset.FromUnixTimeSeconds(result.Timestamp);
            var puzzleTimestamp = DateTimeOffset.FromUnixTimeSeconds(leaderboard.StartTimestamp + (day - 1) * 86400);
            var timeSpan = resultTimestamp - puzzleTimestamp;

            return timeSpan.Days > 0
                ? $"{timeSpan.Days}d+"
                : $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }

        return string.Empty;
    }

    private static string GetFormattedPart2DiffTime(
        Dictionary<int, Result> resultsByPart)
    {
        var part1ResultExists = resultsByPart.TryGetValue(1, out var part1Result);
        var part2ResultExists = resultsByPart.TryGetValue(2, out var part2Result);

        if (!part1ResultExists || !part2ResultExists)
        {
            return string.Empty;
        }

        var part1ResultTimestamp = DateTimeOffset.FromUnixTimeSeconds(part1Result.Timestamp);
        var part2ResultTimestamp = DateTimeOffset.FromUnixTimeSeconds(part2Result.Timestamp);

        var timeSpan = part2ResultTimestamp - part1ResultTimestamp;

        return timeSpan.Days > 0
            ? $"{timeSpan.Days}d+"
            : $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    private sealed record Member(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("stars")] int Stars,
        [property: JsonPropertyName("local_score")]
        int Score,
        [property: JsonPropertyName("completion_day_level")]
        Dictionary<int, Dictionary<int, Result>> Results
    );

    private sealed record Result(
        [property: JsonPropertyName("get_star_ts")]
        long Timestamp
    );

    private sealed record Leaderboard(
        [property: JsonPropertyName("num_days")]
        int Days,
        [property: JsonPropertyName("day1_ts")]
        long StartTimestamp,
        [property: JsonPropertyName("members")]
        Dictionary<string, Member> Members
    );
}