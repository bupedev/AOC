using AOC.Utilities;

namespace AOC.Y2021;

public class D03BinaryDiagnostic() : Solution(2021, 3) {
    protected override object GetPart1Result(string input) {
        var gammaRate = GetRate(input, '1');
        var epsilonRate = GetRate(input, '0');

        return gammaRate * epsilonRate;
    }

    private int GetRate(string input, char targetBit) {
        var rateBits = input
            .ToGrid()
            .AsColumns()
            .Select(
                column => GetBitByCommonality(column, Commonality.Most) == targetBit
                    ? "1"
                    : "0"
            )
            .Join();

        return Convert.ToInt32(rateBits, 2);
    }

    protected override object GetPart2Result(string input) {
        var oxygenRating = GetRating(input, Commonality.Most);
        var carbonRating = GetRating(input, Commonality.Least);

        return oxygenRating * carbonRating;
    }

    private int GetRating(string input, Commonality commonality) {
        var initialGrid = input.ToGrid();

        var ratingBits = Enumerable
            .Range(0, initialGrid.ColumnCount)
            .Aggregate(
                initialGrid,
                (reducedGrid, columnIndex) => {
                    if (reducedGrid.RowCount == 1) return reducedGrid;
                    var targetBit = GetBitByCommonality(reducedGrid[.., columnIndex], commonality);
                    return reducedGrid.FilterRows(candidate => candidate[columnIndex] == targetBit);
                }
            )
            .AsRows()
            .Single();

        return Convert.ToInt32(new string(ratingBits), 2);
    }

    private char GetBitByCommonality(IEnumerable<char> bits, Commonality commonality) {
        var histogram = bits.ToHistogram();

        if (histogram['0'] == histogram['1'])
            return commonality switch {
                Commonality.Least => '0',
                Commonality.Most => '1',
                _ => throw new ArgumentOutOfRangeException(nameof(commonality), commonality, null)
            };

        var bitsByCommonality = histogram.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key);
        return commonality switch {
            Commonality.Least => bitsByCommonality.Last(),
            Commonality.Most => bitsByCommonality.First(),
            _ => throw new ArgumentOutOfRangeException(nameof(commonality), commonality, null)
        };
    }

    private enum Commonality {
        Least,
        Most
    }
}