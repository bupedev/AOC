using System.Collections;
using static AOC.Utilities;

namespace AOC.Y2024;

public class D04CeresSearch() : Solution(2024, 4)
{
    protected override object GetPart1Result(string input)
    {
        return input
            .ToMatrix(t => t)
            .MatrixSelect((matrix, row, column) => Directions.All
                .Count(direction => new string(matrix.ExtractByDirection(row, column, direction, 4)) == "XMAS"))
            .Sum();
    }


    protected override object GetPart2Result(string input)
    {
        var range = (4..10).Iterate();

        return range;

        return input
            .ToMatrix(t => t)
            .SlidingWindow(3, 3)
            .Count(subMatrix => Range2D(2, 2)
                .Sum(coords => Directions.Diagonals.Count(direction =>
                    new string(subMatrix.ExtractByDirection(coords.Row, coords.Column, direction, 3)) == "MAS")) == 2);
    }
}

public sealed class Matrix<T>
{
    private readonly int _columnCount;
    private readonly T[][] _data;
    private readonly int _rowCount;

    public Matrix(T[][] data)
    {
        _data = data;
        _rowCount = _data.Length;
        _columnCount = _data[0].Length;
    }

    public T this[int row, int column] => _data[row][column];

    public T[] this[int row, Range columnRange] => _data[row][columnRange];

    public T[] this[Range rowRange, int column] => _data[rowRange].Select(row => row[column]).ToArray();

    public Matrix<T> this[Range rowRange, Range columnRange] =>
        new(_data[rowRange].Select(row => row[columnRange]).ToArray());

    public IEnumerable<T[]> AsRows()
    {
        return _data;
    }

    public IEnumerable<T[]> AsColumns()
    {
        return (.._columnCount).Iterate().Select(column => this[.., column]);
    }
}

public static class RangeExtensions
{
    public static RangeEnumerator GetEnumerator(this Range range)
    {
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
            throw new ArgumentException("^ operator is not supported in range enumerators", nameof(range));

        return new RangeEnumerator(range.Start.Value, range.End.Value);
    }

    public static IEnumerable<int> Iterate(this Range range)
    {
        foreach (var index in range) yield return index;
    }

    public static bool Includes(this Range range, int index)
    {
        return range.Iterate().Contains(index);
    }

    public struct RangeEnumerator(int start, int end) : IEnumerator<int>
    {
        public int Current { get; private set; } = start - 1;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            return ++Current < end;
        }

        public void Dispose()
        {
        }

        public void Reset()
        {
        }
    }
}