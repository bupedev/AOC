using AOC.Utilities.Grids;
using static AOC.Utilities.Miscellaneous;

namespace AOC.Y2024;

public class D04CeresSearch() : Solution(2024, 4) {
    protected override object GetPart1Result(string input) {
        return input
            .ToMatrix()
            .Transform(
                (matrix, row, column) => CompassDirections.All
                    .Count(direction => new string(matrix.Slice(direction, row, column, 4)) == "XMAS")
            )
            .Flatten()
            .Sum();
    }

    protected override object GetPart2Result(string input) {
        return input
            .ToMatrix()
            .SlidingWindow(3, 3)
            .Select(
                matrix => Combine(matrix.GetIndices(), CompassDirections.Ordinals)
                    .Count(
                        t => {
                            var ((row, column), direction) = t;
                            return new string(matrix.Slice(direction, row, column, 3)) == "MAS";
                        }
                    )
            )
            .Count(masCount => masCount == 2);
    }
}