namespace AOC.Y2024;

public class D01() : Solution(2024, 1) {
    protected override object GetPart1Result(string input) {
        return input
            .SplitOnNewLines()
            .Select(line => line.SplitOnWhitespace().Select(int.Parse))
            .AsMatrix()
            .Transpose()
            .Select(list => list.Order())
            .AsMatrix()
            .Transpose()
            .Select(array => Math.Abs(array[1] - array[0]))
            .Sum();
    }

    protected override object GetPart2Result(string input) {
        var lists = input
            .SplitOnNewLines()
            .Select(line => line.SplitOnWhitespace().Select(int.Parse))
            .AsMatrix()
            .Transpose();

        return lists[0].Select(item => lists[1].Count(t => t == item) * item).Sum();
    }
}