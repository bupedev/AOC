namespace AOC.Utilities;

public enum GridDirection {
    Up,
    Down,
    Left,
    Right,
    UpRight,
    DownRight,
    UpLeft,
    DownLeft
}

public static class GridDirections {
    public static readonly IEnumerable<GridDirection> All = Enum.GetValues<GridDirection>();

    public static readonly IEnumerable<GridDirection> Diagonals = [
        GridDirection.UpRight, GridDirection.UpLeft, GridDirection.DownRight, GridDirection.DownLeft
    ];
}

public static class GridDirectionExtensions {
    public static (int RowOffset, int ColumnOffset) GetGridOffset(this GridDirection gridDirection) {
        return gridDirection switch {
            GridDirection.Up => (0, -1),
            GridDirection.Down => (0, 1),
            GridDirection.Left => (-1, 0),
            GridDirection.Right => (1, 0),
            GridDirection.UpRight => (1, -1),
            GridDirection.DownRight => (1, 1),
            GridDirection.UpLeft => (-1, -1),
            GridDirection.DownLeft => (-1, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(gridDirection), gridDirection, null)
        };
    }
}