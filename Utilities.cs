namespace AOC;

public static class Utilities {
    public static IEnumerable<T[]> SlidingWindow<T>(this IEnumerable<T> source, int windowSize) {
        using var enumerator = source.GetEnumerator();
        var queue = new Queue<T>();

        for (var i = 0; i < windowSize - 1; i++) {
            if (!enumerator.MoveNext()) yield break;
            queue.Enqueue(enumerator.Current);
        }

        while (enumerator.MoveNext()) {
            queue.Enqueue(enumerator.Current);
            yield return queue.ToArray();
            queue.Dequeue();
        }
    }

    public static string[] SplitOnNewLines(this string input) {
        return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitOnWhitespace(this string input) {
        return input.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
    }

    public static T[][] ToMatrix<T>(this string input, Func<char, T> selector) {
        var lines = input.SplitOnNewLines();
        var rowsCount = lines.Length;
        var columnCount = lines[0].Length;

        var matrix = new T[rowsCount][];

        for (var i = 0; i < rowsCount; i++) {
            matrix[i] = new T[columnCount];
            for (var j = 0; j < columnCount; j++) matrix[i][j] = selector(lines[i][j]);
        }

        return matrix;
    }

    public static T[][] Transpose<T>(this T[][] matrix) {
        var rowCount = matrix.GetRowCount();
        var columnCount = matrix.GetColumnCount();

        var columns = new T[columnCount][];
        for (var j = 0; j < columnCount; j++) {
            columns[j] = new T[rowCount];
            for (var i = 0; i < rowCount; i++) columns[j][i] = matrix[i][j];
        }

        return columns;
    }

    public static T[] GetRowByIndex<T>(this T[][] matrix, int rowIndex) {
        return Enumerable
            .Range(0, matrix.GetColumnCount())
            .Select(columnIndex => matrix[rowIndex][columnIndex])
            .ToArray();
    }

    public static T[] GetColumnByIndex<T>(this T[][] matrix, int columnIndex) {
        return Enumerable
            .Range(0, matrix.GetRowCount())
            .Select(rowIndex => matrix[rowIndex][columnIndex])
            .ToArray();
    }

    public static int GetRowCount<T>(this T[][] matrix) {
        return matrix.Length;
    }

    public static int GetColumnCount<T>(this T[][] matrix) {
        return matrix[0].Length;
    }

    public static string Join(this IEnumerable<string> values, string? separator = null) {
        return string.Join(separator ?? string.Empty, values);
    }

    public static IDictionary<T, int> ToHistogram<T>(this IEnumerable<T> values) where T : notnull {
        return values.Aggregate(
            new Dictionary<T, int>(),
            (histogram, value) => {
                if (!histogram.TryAdd(value, 1)) histogram[value]++;
                return histogram;
            }
        );
    }
}