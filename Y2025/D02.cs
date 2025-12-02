using static System.Linq.Enumerable;
using static System.String;

namespace AOC.Y2025;

public class D02() : Solution(2025, 2)
{
    protected override object GetPart1Result(string input)
    {
        return Solve(input, part1: true);
    }

    protected override object GetPart2Result(string input)
    {
        return Solve(input, part1: false);
    }
    
    private static long Solve(string input, bool part1)
    {
        return input
            .Split(',')
            .Select(range => range.Split('-').Select(long.Parse).ToArray())
            .SelectMany(range => LongRange(range[0], range[1]))
            .Where(id => IsInvalid(id.ToString(), part1))
            .Sum();
    }

    private static IEnumerable<long> LongRange(long start, long end)
    {
        for (var i = start; i <= end; i++) yield return i;
    }

    private static bool IsInvalid(string id, bool limit)
    {
        return Range(1, id.Length / 2).Any(i => Concat(Repeat(id[..i], limit ? 2 : id.Length / i)) == id);
    }
}