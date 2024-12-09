using System.Collections;

namespace AOC.Utilities;

public static class RangeExtensions {
    public static Range ToSimpleRange(this Range range, int maxLength = int.MaxValue) {
        var start = range.Start.IsFromEnd ? maxLength - range.Start.Value : range.Start.Value;
        var end = range.End.IsFromEnd ? maxLength - range.End.Value : range.End.Value;

        if (start < 0 || start > maxLength || end < 0 || end > maxLength)
            throw new ArgumentException($"range is malformed (start: {start}, end: {end})");

        return start..end;
    }

    public static int GetLength(this Range range) {
        range.ValidateNotFromEnd();
        return range.End.Value - range.Start.Value;
    }

    public static IEnumerable<Range> Split(this Range range, int splitIndex) {
        range.ValidateNotFromEnd();
        if (splitIndex > range.Start.Value && splitIndex < range.End.Value)
            return [range.Start.Value..splitIndex, splitIndex..range.End.Value];
        return [range];
    }

    public static IEnumerable<Range> SplitOffset(this Range range, int offset) {
        return range.Split(range.Start.Value + offset);
    }

    public static RangeEnumerator GetEnumerator(this Range range) {
        range.ValidateNotFromEnd();
        return new RangeEnumerator(range.Start.Value, range.End.Value);
    }

    public static IEnumerable<int> Iterate(this Range range) {
        foreach (var index in range) yield return index;
    }

    public static bool Includes(this Range range, int index) {
        range.ValidateNotFromEnd();
        return range.Start.Value <= index && index < range.End.Value;
    }

    private static void ValidateNotFromEnd(this Range range) {
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
            throw new ArgumentException("^ operator is not supported in range enumerators", nameof(range));
    }

    public struct RangeEnumerator(int start, int end) : IEnumerator<int> {
        public int Current { get; private set; } = start - 1;

        object IEnumerator.Current => Current;

        public bool MoveNext() {
            return ++Current < end;
        }

        public void Dispose() { }

        public void Reset() { }
    }
}