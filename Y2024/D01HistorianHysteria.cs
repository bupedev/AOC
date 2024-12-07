using AOC.Utilities;

namespace AOC.Y2024;

public class D01HistorianHysteria() : Solution(2024, 1) {
    protected override object GetPart1Result(string input) {
        return input
            .SplitOnNewLines()
            .Select(line => line.SplitOnWhitespace().Select(int.Parse))
            .AsGrid()
            .TransformColumns(list => list.Order())
            .AsRows()
            .Select(array => Math.Abs(array[1] - array[0]))
            .Sum();
    }

    protected override object GetPart2Result(string input) {
        return input
            .SplitOnNewLines()
            .Select(line => line.SplitOnWhitespace().Select(int.Parse))
            .AsGrid()
            .AsColumns()
            .ToArray()
            .Then(
                lists => lists[0]
                    .Select(list0Item => lists[1].Count(list1Item => list1Item == list0Item) * list0Item)
                    .Sum()
            );
    }
}