using AOC.Utilities;

namespace AOC.Y2015;

public class D02IWasToldThereWouldBeNoMath() : Solution(2015, 2) {
    protected override object GetPart1Result(string input) {
        return input
            .SplitOnNewLines()
            .Select(
                line => {
                    var lengths = line.Split('x').Select(int.Parse).ToArray();
                    var areas = lengths.Append(lengths[0]).Pairwise().Select(pair => pair.Product());
                    return areas.Sum(area => 2 * area) + areas.Min();
                }
            )
            .Sum();
    }

    protected override object GetPart2Result(string input) {
        return input
            .SplitOnNewLines()
            .Select(
                line => {
                    var lengths = line.Split('x').Select(int.Parse).ToArray();
                    var perimeters = lengths.Append(lengths[0]).Pairwise().Select(pair => 2 * pair.Sum());
                    return perimeters.Min() + lengths.Product();
                }
            )
            .Sum();
    }
}