using AOC.Utilities;

namespace AOC.Y2015;

public class D05DoesntHeHaveInternElvesForThis() : Solution(2015, 5) {
    protected override object GetPart1Result(string input) {
        return input
            .SplitOnNewLines()
            .Count(
                line => {
                    return line.Count(c => c is 'a' or 'e' or 'i' or 'o' or 'u') >= 3 &&
                           line.Pairwise().Any(pair => pair.Same()) &&
                           !line
                               .Pairwise()
                               .Select(p => new string([p.First, p.Second]))
                               .Any(p => p is "ab" or "cd" or "pq" or "xy");
                }
            );
    }

    protected override object GetPart2Result(string input) {
        return input
            .SplitOnNewLines()
            .Count(
                line => {
                    return line.SlidingWindow(3).Any(triple => triple[0] == triple[2]) &&
                           line
                               .Pairwise()
                               .WithIndices()
                               .GroupBy(p => p.Value)
                               .Any(
                                   g => g
                                       .Pairwise()
                                       .Any(i => i.Second.Index - i.First.Index > 1)
                               );
                }
            );
    }
}