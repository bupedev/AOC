using AOC.Utilities;
using AOC.Utilities.Grids;

namespace AOC.Y2025;

public class D04() : Solution(2025, 4)
{
    protected override object GetPart1Result(string input)
    {
        return FindAccessibleRolls(input.ToGrid()).Count();
    }
    
    protected override object GetPart2Result(string input)
    {
        return RemoveAccessibleRolls(input.ToGrid(), 0);

        static int RemoveAccessibleRolls(Grid<char> grid, int removedRollCount)
        {
            var indices = FindAccessibleRolls(grid).ToHashSet();

            return indices.Count == 0
                ? removedRollCount
                : RemoveAccessibleRolls(
                    grid.Transform((g, i) => indices.Contains(i) ? '.' : g[i]),
                    removedRollCount + indices.Count);
        }
    }
    
    private static IEnumerable<GridIndex> FindAccessibleRolls(Grid<char> grid)
    {
        return grid.FindIndices((g, i) =>
            g[i] == '@' &&
            Neighborhoods
                .Moore(i, 1)
                .Where(g.InRange)
                .Count(n => g[n] == '@') < 4
        );
    }
}