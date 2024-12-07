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
    public static GridIndex GetGridOffset(this CompassDirection compassDirection) {
        return compassDirection switch {
            CompassDirection.North => new GridIndex(0, -1),
            CompassDirection.NorthEast => new GridIndex(1, -1),
            CompassDirection.East => new GridIndex(1, 0),
            CompassDirection.SouthEast => new GridIndex(1, 1),
            CompassDirection.South => new GridIndex(0, 1),
            CompassDirection.SouthWest => new GridIndex(-1, 1),
            CompassDirection.West => new GridIndex(-1, 0),
            CompassDirection.NorthWest => new GridIndex(-1, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(compassDirection), compassDirection, null)
        };
    }
}