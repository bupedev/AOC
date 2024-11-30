namespace AOC;

public class D00Example() : Solution(0, 0) {
    protected override object GetPart1Result(string input) {
        return new { Lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries) };
    }

    protected override object GetPart2Result(string input) {
        return new { CharCount = input.Length };
    }
}