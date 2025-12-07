using AOC.Utilities;
using AOC.Utilities.Grids;

namespace AOC.Y2025;

public class D07() : Solution(2025, 7)
{
    protected override object GetPart1Result(string input)
    {
        return AnalyzeBeams(input).Splits;
    }
    
    protected override object GetPart2Result(string input)
    {
        return AnalyzeBeams(input).Worlds;
    }

    private (long Splits, long Worlds) AnalyzeBeams(string input)
    {
        var splitCache = new HashSet<GridIndex>();
        var worldsCache = new Dictionary<GridIndex, long>();
        
        var grid = input.ToGrid();
        var start = grid.FindIndices((g, i) => g[i] == 'S').Single();

        Analyze(start);
        
        return (splitCache.Count, worldsCache[start]);

        long Analyze(GridIndex beam)
        {
            if (!worldsCache.ContainsKey(beam))
            {
                worldsCache[beam] = Progress(beam);
            }
            
            return worldsCache[beam];
        }

        long Progress(GridIndex beam)
        {
            var below = beam + (1, 0);

            if (!grid.InRange(below))
            {
                return 1;
            }

            switch (grid[below])
            {
                case '^':
                {
                    splitCache.Add(below);
                    return Analyze(below - (0, 1)) + Analyze(below + (0, 1));
                }
                default:
                {
                    return Analyze(below);
                }
            }
        }
    }
}