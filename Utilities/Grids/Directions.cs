namespace AOC.Utilities.Grids;

public enum CompassDirection {
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}

public static class CompassDirections {
    public static readonly IEnumerable<CompassDirection> All = Enum.GetValues<CompassDirection>();

    public static readonly IEnumerable<CompassDirection> Cardinals = [
        CompassDirection.North, CompassDirection.East, CompassDirection.South, CompassDirection.West
    ];

    public static readonly IEnumerable<CompassDirection> Ordinals = [
        CompassDirection.NorthEast, CompassDirection.NorthWest, CompassDirection.SouthEast, CompassDirection.SouthWest
    ];
}

public static class CompassDirectionExtensions {
    public static (int RowOffset, int ColumnOffset) GetGridOffset(this CompassDirection compassDirection) {
        return compassDirection switch {
            CompassDirection.North => (0, -1),
            CompassDirection.NorthEast => (1, -1),
            CompassDirection.East => (1, 0),
            CompassDirection.SouthEast => (1, 1),
            CompassDirection.South => (0, 1),
            CompassDirection.SouthWest => (-1, 1),
            CompassDirection.West => (-1, 0),
            CompassDirection.NorthWest => (-1, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(compassDirection), compassDirection, null)
        };
    }
}