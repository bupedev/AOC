using AOC.Utilities;
using AOC.Utilities.Grids;

namespace AOC.Y2024;

public class D12() : Solution(2024, 12)
{
    protected override object GetPart1Result(string input)
    {
        return input.ToGrid().Then(map =>
        {
            return GetRegions(map).Select(region =>
            {
                var area = region.Count();
                var perimeter = region.Sum(tile =>
                    CompassDirections.Cardinals.Select(dir => dir.GetGridOffset() + tile)
                        .Count(adj => !map.InRange(adj) || map[adj] != map[tile]));

                return area * perimeter;
            }).Sum();
        });
    }

    protected override object GetPart2Result(string input)
    {
        return input.ToGrid().Then(map => GetRegions(map).Select(region =>
        {
            var area = region.Count();
            var perimeter = region.Where(tile =>
                                    !map.InRange(CompassDirection.North.GetGridOffset() + tile) ||
                                    (map[CompassDirection.North.GetGridOffset() + tile] != map[tile]))
                                .GroupBy(tile => tile.Row)
                                .Select(row => CountContiguousIntegers(
                                    row
                                        .Select(tile => tile.Column))).Sum() +
                            region.Where(tile =>
                                    !map.InRange(CompassDirection.South.GetGridOffset() + tile) ||
                                    (map[CompassDirection.South.GetGridOffset() + tile] != map[tile]))
                                .GroupBy(tile => tile.Row)
                                .Select(row => CountContiguousIntegers(
                                    row
                                        .Select(tile => tile.Column))).Sum() +
                            region.Where(tile =>
                                    !map.InRange(CompassDirection.East.GetGridOffset() + tile) ||
                                    (map[CompassDirection.East.GetGridOffset() + tile] != map[tile]))
                                .GroupBy(tile => tile.Column)
                                .Select(row => CountContiguousIntegers(
                                    row
                                        .Select(tile => tile.Row))).Sum() +
                            region.Where(tile =>
                                    !map.InRange(CompassDirection.West.GetGridOffset() + tile) ||
                                    (map[CompassDirection.West.GetGridOffset() + tile] != map[tile]))
                                .GroupBy(tile => tile.Column)
                                .Select(row => CountContiguousIntegers(
                                    row
                                        .Select(tile => tile.Row))).Sum();



            return area * perimeter;
        }).Sum());
    }
    
    private IEnumerable<HashSet<GridIndex>> GetRegions(Grid<char> map)
    {
        return map.GetIndices().Aggregate((Visited: new HashSet<GridIndex>(), Regions: new List<HashSet<GridIndex>>()),
            (state, index) =>
            {
                if (state.Visited.Contains(index)) return state;
                var region = map.CardinalSearch(index,
                    (grid, prev, next) => grid.InRange(next) && grid[prev] == grid[next]).ToHashSet();
                state.Visited.UnionWith(region);
                state.Regions.Add(region);
                return state;
            }).Regions;
    }

    private static int CountContiguousIntegers(IEnumerable<int> values)
    {
        return values.OrderBy(x => x).Pairwise().Count(pair => pair.Second != pair.First + 1) + 1;
    }
}