using AOC.Utilities;

namespace AOC.Y2025;

public class D06() : Solution(2025, 6)
{
    protected override object GetPart1Result(string input)
    {
        var lines = input.SplitOnNewLines();
        var ops = lines[^1].SplitOnWhitespace();
        
        var numbers = lines[..^1]
            .Select(x => x.SplitOnWhitespace().Select(long.Parse))
            .AsGrid()
            .AsColumns()
            .ToArray();
        
        return Calculate(numbers, ops);
    }
    
    protected override object GetPart2Result(string input)
    {
        var lines = input.SplitOnNewLines();
        var ops = lines[^1].SplitOnWhitespace().Reverse().ToArray();
        
        var numbers = lines[..^1]
            .AsGrid()
            .AsColumns()
            .Reverse()
            .Select(r => long.TryParse(string.Join("", r).Trim(), out var num) ? num : 0)
            .Split(num => num == 0)
            .ToJaggedArray();

        return Calculate(numbers, ops);
    }

    private static long Calculate(long[][] numbers, string[] operators)
    {
        return numbers.Index().Sum(sequence =>
        {
            var op = operators[sequence.Index];
            return op switch
            {
                "+" => sequence.Item.Sum(),
                "*" => sequence.Item.Product(),
                _ => throw new InvalidOperationException()
            };
        });
    }
}