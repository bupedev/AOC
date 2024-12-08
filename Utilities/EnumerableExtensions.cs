using System.Numerics;

namespace AOC.Utilities;

public static class EnumerableExtensions {
    public static IEnumerable<(T First, T Second)> Pairwise<T>(this IEnumerable<T> source) {
        return source.SlidingWindow(2).Select(pair => (pair[0], pair[1]));
    }

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

    public static IDictionary<T, int> ToHistogram<T>(this IEnumerable<T> values) where T : notnull {
        return values.Aggregate(
            new Dictionary<T, int>(),
            (histogram, value) => {
                if (!histogram.TryAdd(value, 1)) histogram[value]++;
                return histogram;
            }
        );
    }

    public static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> source, int indexToRemove) {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source
            .Select(
                (item, index) => new {
                    Item = item,
                    Index = index
                }
            )
            .Where(x => x.Index != indexToRemove)
            .Select(x => x.Item);
    }

    public static bool IsAscending<T>(this IEnumerable<T> numbers) where T : IComparable<T> {
        return !numbers
            .Pairwise()
            .Select(pair => pair.Second.CompareTo(pair.First) <= 0)
            .Any(isDescendingOrEqual => isDescendingOrEqual);
    }

    public static bool IsDescending<T>(this IEnumerable<T> numbers) where T : IComparable<T> {
        return !numbers
            .Pairwise()
            .Select(pair => pair.Second.CompareTo(pair.First) >= 0)
            .Any(isAscendingOrEqual => isAscendingOrEqual);
    }

    public static IEnumerable<(T First, T Second)> GetUniquePairs<T>(this IEnumerable<T> items) {
        var array = items.ToArray();
        foreach (var i in ..array.Length)
        foreach (var j in (i + 1)..array.Length)
            yield return (array[i], array[j]);
    }

    public static IEnumerable<(T Value, int Index)> WithIndices<T>(this IEnumerable<T> items) {
        return items.Select((item, index) => (item, index));
    }

    public static T Product<T>(this IEnumerable<T> items) where T : INumber<T> {
        return items.Aggregate((prod, value) => prod * value);
    }

    public static TResult Product<TItem, TResult>(this IEnumerable<TItem> items, Func<TItem, TResult> selector)
        where TResult : INumber<TResult> {
        return items.Select(selector).Aggregate((prod, value) => prod * value);
    }

    public static IEnumerable<TResult> Scan<TIn, TResult>(
        this IEnumerable<TIn> items,
        TResult seed,
        Func<TResult, TIn, TResult> accumulator
    ) {
        var current = seed;
        yield return current;

        foreach (var item in items) {
            current = accumulator(current, item);
            yield return current;
        }
    }

    public static IEnumerable<IEnumerable<TResult>> Split<TIn, TGroup, TResult>(
        this IEnumerable<TIn> items,
        Func<TIn, TGroup> grouper,
        Func<TIn, TResult> selector
    ) {
        return items.GroupBy(grouper).Select(group => group.Select(selector));
    }
}