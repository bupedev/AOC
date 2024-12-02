namespace AOC.Y2024;

public class D02RedNosedReports() : Solution(2024, 2) {
    protected override object GetPart1Result(string input) {
        return input
            .SplitOnNewLines()
            .Select(line => line.SplitOnWhitespace().Select(int.Parse).ToArray())
            .Count(IsSafe);
    }

    protected override object GetPart2Result(string input) {
        return input
            .SplitOnNewLines()
            .Select(line => line.SplitOnWhitespace().Select(int.Parse).ToArray())
            .Count(
                line => IsSafe(line) ||
                        Enumerable
                            .Range(0, line.Length)
                            .Select(line.RemoveAt)
                            .Any(IsSafe)
            );
    }

    private static bool IsLowVariance(IEnumerable<int> numbers) {
        return !numbers
            .Pairwise()
            .Select(pair => Math.Abs(pair.First - pair.Second))
            .Select(variance => variance is < 1 or > 3)
            .Any(isHighVariance => isHighVariance);
    }

    private static bool IsSafe(IEnumerable<int> numbers) {
        var array = numbers.ToArray();
        return (array.IsAscending() || array.IsDescending()) && IsLowVariance(array);
    }
}