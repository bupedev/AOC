using System.Numerics;

namespace AOC.Utilities;

public static class TupleExtensions {
    public static T Sum<T>(this (T, T) tuple) where T : INumber<T> {
        return tuple.Item1 + tuple.Item2;
    }

    public static T Sum<T>(this (T, T, T) tuple) where T : INumber<T> {
        return tuple.Item1 + tuple.Item2 + tuple.Item3;
    }

    public static T Sum<T>(this (T, T, T, T) tuple) where T : INumber<T> {
        return tuple.Item1 + tuple.Item2 + tuple.Item3 + tuple.Item4;
    }

    public static T Product<T>(this (T, T) tuple) where T : INumber<T> {
        return tuple.Item1 * tuple.Item2;
    }

    public static T Product<T>(this (T, T, T) tuple) where T : INumber<T> {
        return tuple.Item1 * tuple.Item2 * tuple.Item3;
    }

    public static T Product<T>(this (T, T, T, T) tuple) where T : INumber<T> {
        return tuple.Item1 * tuple.Item2 * tuple.Item3 * tuple.Item4;
    }
}