using AOC.Utilities;

namespace AOC.Y2025;

public class D05() : Solution(2025, 5)
{
    protected override object GetPart1Result(string input)
    {
        var (ranges, ids) = Parse(input);
        
        return ids.Count(i => ranges.Any(r => r.First <= i && i <= r.Last));
    }
    
    protected override object GetPart2Result(string input)
    {
        var (ranges, _) = Parse(input);

        return ranges
            .OrderBy(r => r.First)
            .ThenByDescending(r => r.Last)
            .Aggregate((Count: 0L, PreviousLast: 0L), (state, range) =>
            {
                if (range.Last < state.PreviousLast)
                {
                    return state;
                }

                return (
                    Count: state.Count + range.Last - Math.Max(state.PreviousLast, range.First) + 1,
                    PreviousLast: range.Last + 1
                );
            }).Count;
    }
    
    private ((long First, long Last)[] Ranges, long[] Ids) Parse(string input)
    {
        var sections = input.SplitOnDoubleNewLines();
        
        var ranges = sections[0]
            .SplitOnNewLines()
            .Select(line => line.Split('-').Select(long.Parse).ToArray())
            .Select(r => (First: r[0], Last: r[1]))
            .ToArray();
        
        var ids = sections[1].SplitOnNewLines().Select(long.Parse).ToArray();

        return (ranges, ids);
    }
}