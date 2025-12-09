using AOC.Utilities;

namespace AOC.Y2025;

public class D08() : Solution(2025, 8)
{
    protected override object GetPart1Result(string input)
    {
        var circuits = new List<HashSet<int>>();
        var boxes = GetBoxes(input);
        var boxPairs = GetBoxPairsByDistance(boxes);

        foreach (var boxPair in boxPairs.Take(1000))
        {
            ConnectBoxes(boxPair, circuits);
        }

        return circuits.Select(c => c.Count).OrderDescending().Take(3).Product();
    }

    protected override object GetPart2Result(string input)
    {
        var circuits = new List<HashSet<int>>();
        var boxes = GetBoxes(input);
        var boxPairs = GetBoxPairsByDistance(boxes).AsEnumerable();

        foreach (var boxPair in boxPairs)
        {
            ConnectBoxes(boxPair, circuits);
            if (circuits.Count == 1 && circuits[0].Count == boxes.Length)
            {
                return boxPair.A.Position.X * boxPair.B.Position.X;
            }
        }

        return double.MaxValue;
    }

    private sealed record Box(int Index, VecD3 Position);

    private static Box[] GetBoxes(string input)
    {
        return input
            .SplitOnNewLines()
            .Select(line => line.Split(',').Select(double.Parse).ToArray())
            .Index()
            .Select(arr => new Box(arr.Index, arr.Item))
            .ToArray();
    }

    private static (Box A, Box B)[] GetBoxPairsByDistance(Box[] boxes)
    {
        return boxes
            .GetUniquePairs()
            .OrderBy(p => VecD3.EuclideanDistance(p.First.Position, p.Second.Position))
            .ToArray();
    }

    private static void ConnectBoxes((Box A, Box B) boxPair, List<HashSet<int>> circuits)
    {
        var aSet = circuits.SingleOrDefault(c => c.Contains(boxPair.A.Index));
        var bSet = circuits.SingleOrDefault(c => c.Contains(boxPair.B.Index));

        if (aSet is not null && bSet is not null)
        {
            if (aSet != bSet)
            {
                circuits.Remove(aSet);
                circuits.Remove(bSet);
                circuits.Add(aSet.Union(bSet).ToHashSet());
            }
        }
        else if (aSet is not null)
        {
            aSet.Add(boxPair.B.Index);
        }
        else if (bSet is not null)
        {
            bSet.Add(boxPair.A.Index);
        }
        else
        {
            circuits.Add([boxPair.A.Index, boxPair.B.Index]);
        }
    }
}