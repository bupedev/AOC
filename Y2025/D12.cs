using AOC.Utilities;

namespace AOC.Y2025;

public class D12() : Solution(2025, 12)
{
    private sealed record Region(int Rows, int Columns, int[] ShapeCounts);

    protected override object GetPart1Result(string input)
    {
        var sections = input.SplitOnDoubleNewLines();
        var shapes = sections[..^1].Select(section => section.SplitOnNewLines()[1..].AsGrid()).ToArray();
        var regions = sections[^1].SplitOnNewLines().Select(line =>
        {
            var cSplit = line.Split(": ");
            var dims = cSplit[0].Split('x');
            var columns = int.Parse(dims[0]);
            var rows = int.Parse(dims[1]);
            var counts = cSplit[1].Split(' ').Select(int.Parse).ToArray();

            return new Region(rows, columns, counts);
        });

        return regions
            .Select(r => 
            (
                Area: r.Rows * r.Columns, 
                PresentCount: r.ShapeCounts.Index()
                    .Sum(count => count.Item * shapes[count.Index].Flatten().Count(x => x == '#'))
            ))
            .Count(t => t.Area > t.PresentCount);
    }

    protected override object GetPart2Result(string input)
    {
        return 0;
    }
}