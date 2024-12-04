namespace AOC;

public static class Utilities
{
    public static IEnumerable<(T First, T Second)> Pairwise<T>(this IEnumerable<T> source)
    {
        return source.SlidingWindow(2).Select(pair => (pair[0], pair[1]));
    }

    public static IEnumerable<T[]> SlidingWindow<T>(this IEnumerable<T> source, int windowSize)
    {
        using var enumerator = source.GetEnumerator();
        var queue = new Queue<T>();

        for (var i = 0; i < windowSize - 1; i++)
        {
            if (!enumerator.MoveNext()) yield break;
            queue.Enqueue(enumerator.Current);
        }

        while (enumerator.MoveNext())
        {
            queue.Enqueue(enumerator.Current);
            yield return queue.ToArray();
            queue.Dequeue();
        }
    }

    public static TOut Then<TIn, TOut>(this TIn value, Func<TIn, TOut> func)
    {
        return func(value);
    }

    public static string[] SplitOnNewLines(this string input)
    {
        return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitOnWhitespace(this string input)
    {
        return input.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
    }

    public static T[][] AsMatrix<T>(this IEnumerable<IEnumerable<T>> values)
    {
        return values.Select(x => x.ToArray()).ToArray();
    }

    public static T[][] Transpose<T>(this IEnumerable<IEnumerable<T>> source)
    {
        var sourceArray = source.AsMatrix();
        if (sourceArray.Length == 0) return [];
        var columnCount = sourceArray.GetColumnCount();
        return Enumerable
            .Range(0, columnCount)
            .Select(columnIndex => sourceArray.Select(row => row[columnIndex]).ToArray())
            .ToArray();
    }

    public static T[][] ToMatrix<T>(this string input, Func<char, T> selector)
    {
        var lines = input.SplitOnNewLines();
        var rowsCount = lines.Length;
        var columnCount = lines[0].Length;

        var matrix = new T[rowsCount][];

        for (var i = 0; i < rowsCount; i++)
        {
            matrix[i] = new T[columnCount];
            for (var j = 0; j < columnCount; j++) matrix[i][j] = selector(lines[i][j]);
        }

        return matrix;
    }

    public static T[] GetRowByIndex<T>(this T[][] matrix, int rowIndex)
    {
        return Enumerable
            .Range(0, matrix.GetColumnCount())
            .Select(columnIndex => matrix[rowIndex][columnIndex])
            .ToArray();
    }

    public static T[] GetColumnByIndex<T>(this T[][] matrix, int columnIndex)
    {
        return Enumerable
            .Range(0, matrix.GetRowCount())
            .Select(rowIndex => matrix[rowIndex][columnIndex])
            .ToArray();
    }

    public static int GetRowCount<T>(this T[][] matrix)
    {
        return matrix.Length;
    }

    public static int GetColumnCount<T>(this T[][] matrix)
    {
        return matrix.Max(row => row.Length);
    }

    public static string Join(this IEnumerable<string> values, string? separator = null)
    {
        return string.Join(separator ?? string.Empty, values);
    }

    public static IDictionary<T, int> ToHistogram<T>(this IEnumerable<T> values) where T : notnull
    {
        return values.Aggregate(
            new Dictionary<T, int>(),
            (histogram, value) =>
            {
                if (!histogram.TryAdd(value, 1)) histogram[value]++;
                return histogram;
            }
        );
    }

    public static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> source, int indexToRemove)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source
            .Select(
                (item, index) => new
                {
                    Item = item,
                    Index = index
                }
            )
            .Where(x => x.Index != indexToRemove)
            .Select(x => x.Item);
    }

    public static bool IsAscending<T>(this IEnumerable<T> numbers) where T : IComparable<T>
    {
        return !numbers
            .Pairwise()
            .Select(pair => pair.Second.CompareTo(pair.First) <= 0)
            .Any(isDescendingOrEqual => isDescendingOrEqual);
    }

    public static bool IsDescending<T>(this IEnumerable<T> numbers) where T : IComparable<T>
    {
        return !numbers
            .Pairwise()
            .Select(pair => pair.Second.CompareTo(pair.First) >= 0)
            .Any(isAscendingOrEqual => isAscendingOrEqual);
    }

    public static IEnumerable<int> Range(int start, int end)
    {
        return Enumerable.Range(start, end);
    }

    public static IEnumerable<(int Row, int Column)> Range2D((int Start, int End) rowRange,
        (int Start, int End) columnRange)
    {
        return Range(rowRange.Start, rowRange.End)
            .SelectMany(row => Range(columnRange.Start, columnRange.End).Select(column => (row, column)));
    }

    public static IEnumerable<(int Row, int Column)> Range2D(int rowEnd, int columnEnd)
    {
        return Range2D((0, rowEnd), (0, columnEnd));
    }

    public static IEnumerable<T[][]> SlidingWindow<T>(this T[][] matrix, int rows, int columns)
    {
        return Range2D(matrix.GetRowCount() - rows + 1, matrix.GetColumnCount() - columns + 1)
            .Select(coord => matrix.ToSubMatrix(rows, columns, coord.Row, coord.Column));
    }

    public static T[][] ToSubMatrix<T>(this T[][] matrix, int rows, int columns, int rowOffset, int columnOffset)
    {
        return matrix[rowOffset..(rows + rowOffset)].Select(row => row[columnOffset..(columns + columnOffset)])
            .ToArray();
    }

    public static IEnumerable<TOut> MatrixSelect<TIn, TOut>(this TIn[][] matrix, Func<TIn[][], int, int, TOut> selector)
    {
        return Range2D(matrix.GetRowCount(), matrix.GetColumnCount())
            .Select(coords => selector(matrix, coords.Row, coords.Column));
    }

    public static bool InRange<T>(this T[][] matrix, int row, int column)
    {
        return row >= 0 && row < matrix.GetRowCount() && column >= 0 && column < matrix.GetColumnCount();
    }

    public static T[] ExtractByDirection<T>(this T[][] matrix, int row, int column, Direction direction, int maxSteps)
    {
        return Range(0, maxSteps).Select(step =>
            {
                var (rowOffset, columnOffset) = direction.GetGridOffset();
                return (Row: row + step * rowOffset, Column: column + step * columnOffset);
            }).Where(coord => matrix.InRange(coord.Row, coord.Column))
            .Select(coord => matrix[coord.Row][coord.Column]).ToArray();
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    UpRight,
    DownRight,
    UpLeft,
    DownLeft
}

public static class Directions
{
    public static IEnumerable<Direction> All = Enum.GetValues<Direction>();

    public static IEnumerable<Direction> Diagonals =
        [Direction.UpRight, Direction.UpLeft, Direction.DownRight, Direction.DownLeft];
}

public static class DirectionExtensions
{
    public static (int RowOffset, int ColumnOffset) GetGridOffset(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            Direction.UpRight => (1, -1),
            Direction.DownRight => (1, 1),
            Direction.UpLeft => (-1, -1),
            Direction.DownLeft => (-1, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}