namespace AOC.Utilities;

public sealed class Matrix<T> {
    private readonly T[][] _data;

    public Matrix(T[][] data) {
        _data = data;
        RowCount = _data.Length;
        ColumnCount = _data[0].Length;
    }

    public int RowCount { get; }

    public int ColumnCount { get; }

    public T this[int row, int column] => _data[row][column];

    public T[] this[int row, Range columnRange] => _data[row][columnRange];

    public T[] this[Range rowRange, int column] => _data[rowRange].Select(row => row[column]).ToArray();

    public Matrix<T> this[Range rowRange, Range columnRange] =>
        new(_data[rowRange].Select(row => row[columnRange]).ToArray());

    public IEnumerable<T[]> AsRows() {
        return _data;
    }

    public IEnumerable<T[]> AsColumns() {
        return (..ColumnCount).Iterate().Select(column => this[.., column]);
    }

    public IEnumerable<(int Row, int Column)> GetIndices() {
        return GetIndices(.., ..);
    }

    public IEnumerable<(int Row, int Column)> GetIndices(Range rows, Range columns) {
        return rows
            .ToSimpleRange(RowCount)
            .Iterate()
            .SelectMany(row => columns.ToSimpleRange(ColumnCount).Iterate().Select(column => (row, column)));
    }

    public bool InRange(int row, int column) {
        return (..RowCount).Includes(row) && (..ColumnCount).Includes(column);
    }

    public Matrix<T> Transpose() {
        return new Matrix<T>(AsColumns().ToArray());
    }

    public Matrix<T> FilterRows(Func<T[], bool> rowFilter) {
        return new Matrix<T>(AsRows().Where(rowFilter).ToArray());
    }

    public Matrix<T> FilterColumns(Func<T[], bool> columnFilter) {
        return new Matrix<T>(AsColumns().Where(columnFilter).ToArray()).Transpose();
    }

    public Matrix<T> TransformRows(Func<T[], IEnumerable<T>> rowTransformation) {
        return new Matrix<T>(AsRows().Select(rowTransformation).Select(row => row.ToArray()).ToArray());
    }

    public Matrix<T> TransformColumns(Func<T[], IEnumerable<T>> columnTransformation) {
        return new Matrix<T>(AsColumns().Select(columnTransformation).Select(column => column.ToArray()).ToArray())
            .Transpose();
    }

    public T[] Slice(GridDirection gridDirection, int row, int column, int length) {
        return (..length)
            .Iterate()
            .Select(
                step => {
                    var (rowOffset, columnOffset) = gridDirection.GetGridOffset();
                    return (Row: row + step * rowOffset, Column: column + step * columnOffset);
                }
            )
            .Where(coord => InRange(coord.Row, coord.Column))
            .Select(coord => this[coord.Row, coord.Column])
            .ToArray();
    }

    public Matrix<TOut> Transform<TOut>(Func<Matrix<T>, int, int, TOut> transformation) {
        return new Matrix<TOut>(
            (..RowCount)
            .Iterate()
            .Select(
                row => (..ColumnCount)
                    .Iterate()
                    .Select(column => transformation(this, row, column))
                    .ToArray()
            )
            .ToArray()
        );
    }

    public IEnumerable<T> Flatten() {
        return GetIndices().Select(index => this[index.Row, index.Column]);
    }

    public IEnumerable<Matrix<T>> SlidingWindow(int rowCount, int columnCount) {
        return GetIndices(..^(rowCount - 1), ..^(columnCount - 1))
            .Select(coord => this[coord.Row..(coord.Row + rowCount), coord.Column..(coord.Column + columnCount)]);
    }

    public override string ToString() {
        var stringMatrix = Transform((matrix, row, column) => matrix[row, column]?.ToString() ?? string.Empty);
        var columnWidth = stringMatrix.AsColumns().SelectMany(column => column.Select(text => text.Length)).Max();

        return stringMatrix
            .AsRows()
            .Select(
                row => row
                    .Select((value, index) => value.PadLeft(columnWidth))
                    .Join(", ")
            )
            .Join(Environment.NewLine);
    }
}