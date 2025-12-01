using AOC.Utilities;

namespace AOC.Y2025;

public class D01() : Solution(2025, 1)
{
    protected override object GetPart1Result(string input)
    {
        return input
            .SplitOnNewLines()
            .Select(line => (Direction: line[0], Steps: int.Parse(line[1..])))
            .Select(cmd => cmd.Steps * cmd.Direction switch { 'R' => 1, 'L' => -1 })
            .Scan(50, (acc, steps) => (acc + steps).Modulo(100))
            .Count(end => end == 0);
    }

    protected override object GetPart2Result(string input)
    {
        return input
            .SplitOnNewLines()
            .Select(line => (Direction: line[0], Steps: int.Parse(line[1..])))
            .SelectMany(cmd => (..cmd.Steps).Iterate().Select(_ => cmd.Direction switch { 'R' => 1, 'L' => -1 }))
            .Scan(50, (acc, steps) => (acc + steps).Modulo(100))
            .Count(end => end == 0);
    }
}