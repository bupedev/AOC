namespace AOC.Y2021;

public class D01SonarSweep() : Solution(2021, 1) {
    protected override object GetPart1Result(string input) {
        return input
            .SplitOnWhitespace()
            .Select(int.Parse)
            .SlidingWindow(2)
            .Count(p => p[1] > p[0]);
    }

    protected override object GetPart2Result(string input) {
        return input
            .SplitOnWhitespace()
            .Select(int.Parse)
            .SlidingWindow(3)
            .Select(t => t.Sum())
            .SlidingWindow(2)
            .Count(p => p[1] > p[0]);
    }
}