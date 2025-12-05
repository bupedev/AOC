using System.Diagnostics;

namespace AOC;

public static class CacheService
{
    public static bool TryGetPuzzleInput(int year, int day, out string input) {
        Ensure(CacheDirectory);

        input = string.Empty;

        if (TryReadFromFile(GetPuzzlePropertyPath(year, day, "input"), out input)) return true;

        if (Client.TryReadPuzzle(year, day, out input) == ClientResponseType.Pass) {
            WriteToFile(GetPuzzlePropertyPath(year, day, "input"), input);
            return true;
        }

        return false;
    }
    
    public static bool TryGetLeaderboard(int year, int leaderboardId, out string json) {
        Ensure(CacheDirectory);

        json = string.Empty;
        
        switch (Client.TryReadLeaderboard(year, leaderboardId, out json))
        {
            case ClientResponseType.Pass:
            {
                WriteToFile(GetLeaderboardPath(year, leaderboardId), json);
                
                return true;
            }
            case ClientResponseType.Fail:
            {
                return false;
            }
            case ClientResponseType.Throttle:
            {
                return TryReadFromFile(GetLeaderboardPath(year, leaderboardId), out json);
            }
            default:
                throw new UnreachableException();
        }
    }

    private static string GetPuzzlePropertyPath(int year, int day, string fileName) {
        var puzzleDirectory = GetPuzzleDirectory(year, day);
        Ensure(puzzleDirectory);
        return Path.Combine(puzzleDirectory, fileName);
    }

    private static string GetPuzzleDirectory(int year, int day) {
        return Path.Combine(CacheDirectory, $"{year}-{day}");
    }

    private static string GetLeaderboardPath(int year, int leaderboardId) {
        var yearDirectory = Path.Combine(CacheDirectory, year.ToString());
        Ensure(yearDirectory);
        return Path.Combine(yearDirectory, $"{leaderboardId}.json");
    }
    
    private static readonly string CacheDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "advent-of-code"
    );
    
    private static void WriteToFile(string filePath, string contents) {
        File.WriteAllText(filePath, contents);
    }

    private static bool TryReadFromFile(string filePath, out string contents) {
        var exists = File.Exists(filePath);
        contents = exists ? File.ReadAllText(filePath) : string.Empty;
        return exists;
    }

    private static void Ensure(string directoryPath) {
        Directory.CreateDirectory(directoryPath);
    }
}