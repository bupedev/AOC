using AOC.Utilities;
using AOC.Utilities.Grids;

namespace AOC.Y2024;

public class D06GuardGallivant() : Solution(2024, 6) {
    protected override object GetPart1Result(string input) {
        return input
            .ToGrid()
            .Transform(ConvertToTile)
            .Then(
                map => {
                    var startingPoint = map.FindIndices(tile => tile is Tile.Man).Single();
                    TryWalkPath(map, startingPoint, out var path);
                    return path.Distinct().Count();
                }
            );
    }

    protected override object GetPart2Result(string input) {
        return input
            .ToGrid()
            .Transform(ConvertToTile)
            .Then(
                map => {
                    var startingPoint = map.FindIndices(tile => tile is Tile.Man).Single();
                    TryWalkPath(map, startingPoint, out var path);
                    return path
                        .Distinct()
                        .Select(
                            gridIndex => {
                                var mapPermutation = map.Clone();
                                mapPermutation[gridIndex] = Tile.NewObstacle;
                                return mapPermutation;
                            }
                        )
                        .Count(mapPermutation => !TryWalkPath(mapPermutation, startingPoint, out _));
                }
            );
    }

    private static bool TryWalkPath(Grid<Tile> map, GridIndex startingPosition, out List<GridIndex> path) {
        path = [];

        var current = startingPosition;
        var direction = CompassDirection.North;
        var stateSet = new HashSet<(int, int, CompassDirection)>();

        while (map.InRange(current)) {
            if (!stateSet.Add((current.Row, current.Column, direction))) return false;
            path.Add(current);
            var next = current + direction.GetGridOffset();
            if (map.InRange(next) && map[next] is Tile.Obstacle or Tile.NewObstacle)
                direction = direction.Turn(TurnDirection.Right);
            else
                current = next;
        }

        return true;
    }

    private static Tile ConvertToTile(char character) {
        return character switch {
            '.' => Tile.Blank,
            '#' => Tile.Obstacle,
            '^' => Tile.Man,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private enum Tile {
        Blank,
        Obstacle,
        NewObstacle,
        Man
    }
}