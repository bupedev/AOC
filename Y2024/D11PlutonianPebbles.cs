using AOC.Utilities;

namespace AOC.Y2024;

public class D11PlutonianPebbles() : Solution(2024, 11) {
    private static readonly Dictionary<(long, int), long> StoneCountByBlinkByValue = new();

    protected override object GetPart1Result(string input) {
        return input.SplitOnWhitespace().Select(long.Parse).Select(num => CountStonesDynamically(num, 25)).Sum();
    }

    protected override object GetPart2Result(string input) {
        return input.SplitOnWhitespace().Select(long.Parse).Select(num => CountStonesDynamically(num, 75)).Sum();
    }

    private static long CountStonesDynamically(long stoneValue, int generation) {
        return generation == 0
            ? 1
            : StoneCountByBlinkByValue
                .GetValueOrAssignDefault(
                    (stoneValue, generation),
                    () =>
                        BlinkAtStone(stoneValue)
                            .Select(newStoneValue => CountStonesDynamically(newStoneValue, generation - 1))
                            .Sum()
                );
    }

    private static long[] BlinkAtStone(long number) {
        if (number is 0) return [1];
        var digits = number.CountDigits();
        return digits % 2 == 0 ? number.SplitByDigits(digits / 2) : [number * 2024];
    }
}