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
}