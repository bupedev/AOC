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
}