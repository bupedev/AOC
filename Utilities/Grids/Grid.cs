namespace AOC.Utilities.Grids;

public sealed record GridIndex(int Row, int Column)
{
    public static implicit operator GridIndex((int Row, int Column) values)
    {
        return new GridIndex(values.Row, values.Column);
    }

    public static implicit operator (int Row, int Column)(GridIndex index)
    {
        return (index.Row, index.Column);
    }

    public static GridIndex operator +(GridIndex a, GridIndex b)
    {
        return (a.Row + b.Row, a.Column + b.Column);
    }

    public static GridIndex operator -(GridIndex a, GridIndex b)
    {
        return (a.Row - b.Row, a.Column - b.Column);
    }

    public static GridIndex operator *(int a, GridIndex b)
    {
        return new GridIndex(a * b.Row, a * b.Column);
    }
}

public sealed class Grid<T>
{
    private readonly T[][] _data;

    public Grid(T[][] data)
    {
        _data = data;
        RowCount = _data.Length;
        ColumnCount = _data[0].Length;
    }

    public int RowCount { get; }

    public int ColumnCount { get; }

    public T this[GridIndex index]
    {
        get => this[index.Row, index.Column];
        set => this[index.Row, index.Column] = value;
    }

    public T this[int row, int column]
    {
        get => _data[row][column];
        set => _data[row][column] = value;
    }

    public T[] this[int row, Range columnRange] => _data[row][columnRange];

    public T[] this[Range rowRange, int column] => _data[rowRange].Select(row => row[column]).ToArray();

    public Grid<T> this[Range rowRange, Range columnRange] =>
        new(_data[rowRange].Select(row => row[columnRange]).ToArray());

    public Grid<T> Clone()
    {
        return new Grid<T>(_data.Select(row => row.Select(columnValue => columnValue).ToArray()).ToArray());
    }

    public IEnumerable<T[]> AsRows()
    {
        return _data;
    }

    public IEnumerable<T[]> AsColumns()
    {
        return (..ColumnCount).Iterate().Select(column => this[.., column]);
    }

    public IEnumerable<GridIndex> GetIndices()
    {
        return GetIndices(.., ..);
    }

    public IEnumerable<GridIndex> GetIndices(Range rows, Range columns)
    {
        return rows
            .ToSimpleRange(RowCount)
            .Iterate()
            .SelectMany(
                row => columns.ToSimpleRange(ColumnCount).Iterate().Select(column => new GridIndex(row, column))
            );
    }

    public bool InRange(GridIndex index)
    {
        return (..RowCount).Includes(index.Row) && (..ColumnCount).Includes(index.Column);
    }

    public Grid<T> Transpose()
    {
        return new Grid<T>(AsColumns().ToArray());
    }

    public Grid<T> FilterRows(Func<T[], bool> rowFilter)
    {
        return new Grid<T>(AsRows().Where(rowFilter).ToArray());
    }

    public Grid<T> FilterColumns(Func<T[], bool> columnFilter)
    {
        return new Grid<T>(AsColumns().Where(columnFilter).ToArray()).Transpose();
    }

    public Grid<T> TransformRows(Func<T[], IEnumerable<T>> rowTransformation)
    {
        return new Grid<T>(AsRows().Select(rowTransformation).Select(row => row.ToArray()).ToArray());
    }

    public Grid<T> TransformColumns(Func<T[], IEnumerable<T>> columnTransformation)
    {
        return new Grid<T>(AsColumns().Select(columnTransformation).Select(column => column.ToArray()).ToArray())
            .Transpose();
    }

    public T[] Slice(CompassDirection compassDirection, GridIndex index, int length)
    {
        return (..length)
            .Iterate()
            .Select(step => step * (index + compassDirection.GetGridOffset()))
            .Where(InRange)
            .Select(stepIndex => this[stepIndex])
            .ToArray();
    }

    public Grid<TOut> Transform<TOut>(Func<Grid<T>, GridIndex, TOut> transformation)
    {
        return new Grid<TOut>(
            (..RowCount)
            .Iterate()
            .Select(
                row => (..ColumnCount)
                    .Iterate()
                    .Select(column => transformation(this, new GridIndex(row, column)))
                    .ToArray()
            )
            .ToArray()
        );
    }

    public Grid<TOut> Transform<TOut>(Func<T, TOut> transformation)
    {
        return Transform<TOut>((grid, index) => transformation(grid[index]));
    }

    public IEnumerable<GridIndex> FindIndices(Func<Grid<T>, GridIndex, bool> predicate)
    {
        return GetIndices().Where(index => predicate(this, index));
    }

    public IEnumerable<GridIndex> FindIndices(Func<T, bool> predicate)
    {
        return FindIndices((grid, index) => predicate(grid[index]));
    }

    public IEnumerable<T> Flatten()
    {
        return GetIndices().Select(index => this[index]);
    }

    public IEnumerable<Grid<T>> SlidingWindow(int rowCount, int columnCount)
    {
        return GetIndices(..^(rowCount - 1), ..^(columnCount - 1))
            .Select(index => this[index.Row..(index.Row + rowCount), index.Column..(index.Column + columnCount)]);
    }

    public IEnumerable<GridIndex> CardinalSearch(GridIndex gridIndex,
        Func<Grid<T>, GridIndex, GridIndex, bool> inclusionPredicate)
    {
        var inclusionSet = new HashSet<GridIndex>();
        var indexQueue = new Queue<GridIndex>();

        inclusionSet.Add(gridIndex);
        indexQueue.Enqueue(gridIndex);

        while (indexQueue.Count > 0)
        {
            var target = indexQueue.Dequeue();
            var candidates = CompassDirections.Cardinals.Select(x => x.GetGridOffset() + target)
                .Where(adj => inclusionPredicate(this, target, adj)).Where(index => !inclusionSet.Contains(index));
            foreach (var candidate in candidates)
            {
                inclusionSet.Add(candidate);
                indexQueue.Enqueue(candidate);
            }
        }
        
        return inclusionSet;
        
    }

    public string Stringify(bool commaDelimitRows = true)
    {
        var stringGrid = Transform((grid, index) => grid[index]?.ToString() ?? string.Empty);
        var columnWidth = stringGrid.AsColumns().SelectMany(column => column.Select(text => text.Length)).Max();

        return stringGrid
            .AsRows()
            .Select(
                row => row
                    .Select(value => value.PadLeft(columnWidth))
                    .Join(commaDelimitRows ? ", " : string.Empty)
            )
            .Join(Environment.NewLine);
    }
}