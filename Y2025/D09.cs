using AOC.Utilities;

namespace AOC.Y2025;

public class D09() : Solution(2025, 9)
{
    protected override object GetPart1Result(string input)
    {
        var indices = input
            .SplitOnNewLines()
            .Select(line => line.Split(',').Select(long.Parse).ToArray())
            .Select(arr => new Coord(arr[1], arr[0]))
            .ToArray();
        
        return indices.GetUniquePairs().Select(p => Rectangle.FromIndices(p.First, p.Second)).Max(r => r.Area);
    }

    protected override object GetPart2Result(string input)
    {
        var indices = input
            .SplitOnNewLines()
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .Select(arr => new Coord(arr[1], arr[0]))
            .ToArray();

        var innerRects = indices.GetUniquePairs().Select(p => Rectangle.FromIndices(p.First, p.Second));
        var edgeRects = indices.Append(indices[0]).Pairwise().Select(p => Rectangle.FromIndices(p.First, p.Second));

        return innerRects
            .OrderByDescending(rect => rect.Area)
            .First(rect => edgeRects.All(edge => !Rectangle.Overlaps(edge, rect)))
            .Area;
    }

    private sealed record Coord(long Row, long Column);
    
    private sealed record Rectangle(long MinRow, long MaxRow, long MinColumn, long MaxColumn)
    {
        public static Rectangle FromIndices(Coord first, Coord second)
        {
            var minRow = Math.Min(first.Row, second.Row);
            var maxRow = Math.Max(first.Row, second.Row);
            var minColumn = Math.Min(first.Column, second.Column);
            var maxColumn = Math.Max(first.Column, second.Column);
            return new Rectangle(minRow, maxRow, minColumn, maxColumn);
        }
        
        public static bool Overlaps(Rectangle first, Rectangle second)
        {
            return !(
                first.MaxColumn <= second.MinColumn || 
                first.MinColumn >= second.MaxColumn || 
                first.MaxRow <= second.MinRow || 
                first.MinRow >= second.MaxRow
            );
        }
    
        public long Area => (MaxRow - MinRow + 1) * (MaxColumn - MinColumn + 1);
    }
}