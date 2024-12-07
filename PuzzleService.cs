namespace AOC;

public static class PuzzleService {
    private static readonly string CacheDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "advent-of-code"
    );

    public static bool TryGetPuzzleInput(int year, int day, out string input) {
        Ensure(CacheDirectory);

        input = string.Empty;

        if (TryReadFromFile(GetPuzzlePropertyPath(year, day, "input"), out input)) return true;

        if (Client.TryReadPropertiesFromServer(year, day, out input) == ClientResponseType.Pass) {
            WriteToFile(GetPuzzlePropertyPath(year, day, "input"), input);
            return true;
        }

        return false;
    }

    private static string GetPuzzlePropertyPath(int year, int day, string fileName) {
        var puzzleDirectory = GetPuzzleDirectory(year, day);
        Ensure(puzzleDirectory);
        return Path.Combine(puzzleDirectory, fileName);
    }

    private static string GetPuzzleDirectory(int year, int day) {
        return Path.Combine(CacheDirectory, $"{year}-{day}");
    }

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