using AOC.Utilities;
using AOC.Utilities.Grids;

namespace AOC.Y2024;

public class D10HoofIt() : Solution(2024, 10) {
    protected override object GetPart1Result(string input) {
        return input
            .ToGrid()
            .Transform(c => int.Parse(c.ToString()))
            .Then(
                map => map
                    .FindIndices(tile => tile == 0)
                    .Select(start => FindTrails(map, start).GroupBy(trail => trail.Last()).Count())
                    .Sum()
            );
    }

    protected override object GetPart2Result(string input) {
        return input
            .ToGrid()
            .Transform(c => int.Parse(c.ToString()))
            .Then(
                map => map
                    .FindIndices(tile => tile == 0)
                    .SelectMany(start => FindTrails(map, start))
                    .Count()
            );
    }

    private List<List<GridIndex>> FindTrails(Grid<int> map, GridIndex start) {
        var trailsFound = new List<List<GridIndex>>();
        FindTrailsRec(map, [start], trailsFound);
        return trailsFound;
    }

    private void FindTrailsRec(
        Grid<int> map,
        List<GridIndex> currentTrail,
        List<List<GridIndex>> trails
    ) {
        var current = currentTrail.Last();
        var nextCandidates = CompassDirections
            .Cardinals
            .Select(dir => current + dir.GetGridOffset())
            .Where(map.InRange)
            .Where(next => map[next] == map[current] + 1);

        foreach (var next in nextCandidates) {
            var newTrail = currentTrail.Append(next).ToList();
            if (map[current] == 8)
                trails.Add(newTrail);
            else
                FindTrailsRec(map, currentTrail.Append(next).ToList(), trails);
        }
    }
}