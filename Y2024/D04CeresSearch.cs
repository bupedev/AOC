using AOC.Utilities.Grids;
using static AOC.Utilities.Miscellaneous;

namespace AOC.Y2024;

public class D04CeresSearch() : Solution(2024, 4) {
    protected override object GetPart1Result(string input) {
        return input
            .ToGrid()
            .Transform(
                (grid, index) => CompassDirections.All
                    .Count(direction => new string(grid.Slice(direction, index, 4)) == "XMAS")
            )
            .Flatten()
            .Sum();
    }

    protected override object GetPart2Result(string input) {
        return input
            .ToGrid()
            .SlidingWindow(3, 3)
            .Select(
                grid => Combine(grid.GetIndices(), CompassDirections.Ordinals)
                    .Count(
                        t => {
                            var (index, direction) = t;
                            return new string(grid.Slice(direction, index, 3)) == "MAS";
                        }
                    )
            )
            .Count(masCount => masCount == 2);
    }
}