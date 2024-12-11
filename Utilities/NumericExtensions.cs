namespace AOC.Utilities;

public static class NumericExtensions {
    public static int CountDigits(this int number) {
        return (int)Math.Floor(Math.Log10(number) + 1);
    }

    public static int[] SplitByDigits(this int number, int digits) {
        var divisor = (int)Math.Pow(10, digits / 2);
        return [number / divisor, number % divisor];
    }

    public static long CountDigits(this long number) {
        return (long)Math.Floor(Math.Log10(number) + 1);
    }

    public static long[] SplitByDigits(this long number, long digits) {
        var divisor = (long)Math.Pow(10, digits);
        return [number / divisor, number % divisor];
    }
}