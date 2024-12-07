using AOC.Utilities.Grids;

namespace AOC.Utilities;

public static class Miscellaneous {
    public static TOut Then<TIn, TOut>(this TIn value, Func<TIn, TOut> func) {
        return func(value);
    }

    public static string[] SplitOnDoubleNewLines(this string input) {
        return input.Split(["\n\n", "\r\n\r\n"], StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitOnNewLines(this string input) {
        return input.Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitOnWhitespace(this string input) {
        return input.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
    }

    public static Grid<T> AsGrid<T>(this IEnumerable<IEnumerable<T>> values) {
        return new Grid<T>(values.Select(x => x.ToArray()).ToArray());
    }

    public static Grid<char> ToGrid(this string input) {
        return new Grid<char>(input.SplitOnNewLines().Select(rowCharacters => rowCharacters.ToArray()).ToArray());
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

    public static Func<T, bool> Not<T>(Func<T, bool> predicate) {
        return x => !predicate(x);
    }

    public static IEnumerable<T[]> GetAllPermutations<T>(this T[] candidateValues, int length) {
        var indices = new int[length];
        var currentArray = new T[length];

        while (true) {
            foreach (var i in ..length) currentArray[i] = candidateValues[indices[i]];
            yield return (T[])currentArray.Clone();

            var position = length - 1;
            while (position >= 0 && indices[position] == candidateValues.Length - 1) {
                indices[position] = 0;
                position--;
            }

            if (position < 0) break;

            indices[position]++;
        }
    }
}