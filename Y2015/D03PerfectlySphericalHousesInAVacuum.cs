using AOC.Utilities;
using AOC.Utilities.Grids;

namespace AOC.Y2015;

public class D03PerfectlySphericalHousesInAVacuum() : Solution(2015, 3) {
    protected override object GetPart1Result(string input) {
        return input
            .Select(GetCompassDirection)
            .Select(direction => direction.GetGridOffset())
            .Scan(new GridIndex(0, 0), (location, direction) => location + direction)
            .Distinct()
            .Count();
    }

    protected override object GetPart2Result(string input) {
        return input
            .Select(GetCompassDirection)
            .Select(direction => direction.GetGridOffset())
            .WithIndices()
            .Split(direction => direction.Index % 2, direction => direction.Value)
            .Select(directions => directions.Scan(new GridIndex(0, 0), (location, direction) => location + direction))
            .SelectMany(t => t)
            .Distinct()
            .Count();
    }

    private static CompassDirection GetCompassDirection(char character) {
        return character switch {
            '^' => CompassDirection.North,
            '>' => CompassDirection.East,
            'v' => CompassDirection.South,
            '<' => CompassDirection.West,
            _ => throw new ArgumentOutOfRangeException(nameof(character), character, null)
        };
    }
}