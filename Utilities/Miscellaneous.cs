namespace AOC.Utilities;

public static class Miscellaneous {
    public static TOut Then<TIn, TOut>(this TIn value, Func<TIn, TOut> func) {
        return func(value);
    }

    public static string[] SplitOnNewLines(this string input) {
        return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitOnWhitespace(this string input) {
        return input.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
    }

    public static Matrix<T> AsMatrix<T>(this IEnumerable<IEnumerable<T>> values) {
        return new Matrix<T>(values.Select(x => x.ToArray()).ToArray());
    }

    public static Matrix<char> ToMatrix(this string input) {
        return new Matrix<char>(input.SplitOnNewLines().Select(rowCharacters => rowCharacters.ToArray()).ToArray());
    }

    public static string Join(this IEnumerable<string> values, string? separator = null) {
        return string.Join(separator ?? string.Empty, values);
    }

    public static IEnumerable<(T1 First, T2 Second)> Combine<T1, T2>(
        IEnumerable<T1> firstSource,
        IEnumerable<T2> secondSource
    ) {
        return firstSource.SelectMany(first => secondSource.Select(second => (first, second)));
    }
}