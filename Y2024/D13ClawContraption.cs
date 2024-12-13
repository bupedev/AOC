using System.Text.RegularExpressions;
using AOC.Utilities;

namespace AOC.Y2024;

public partial class D13ClawContraption() : Solution(2024, 13)
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex NumberRegex();

    protected override object GetPart1Result(string input)
    {
        return input
            .SplitOnDoubleNewLines()
            .Select(ParseMachine)
            .SelectWhereNotNull(CountButtonPresses)
            .Sum(buttonPresses => buttonPresses.X * 3 + buttonPresses.Y);
    }

    protected override object GetPart2Result(string input)
    {
        return input
            .SplitOnDoubleNewLines()
            .Select(ParseMachine)
            .Select(machine => machine with { PrizeLocation = machine.PrizeLocation + 10000000000000 })
            .SelectWhereNotNull(CountButtonPresses)
            .Sum(buttonPresses => buttonPresses.X * 3 + buttonPresses.Y);
    }

    private static Machine ParseMachine(string lineGroup)
    {
        return lineGroup
            .SplitOnNewLines()
            .Select(line => NumberRegex()
                .Matches(line)
                .Select(match => match.Value)
                .Select(double.Parse)
                .ToArray()
            )
            .ToArray().Then(values => 
                new Machine(
                    new Vectors.Vec2<double>(values[0][0], values[0][1]), 
                    new Vectors.Vec2<double>(values[1][0], values[1][1]), 
                    new Vectors.Vec2<double>(values[2][0], values[2][1])
                )
            );
    }

    private static Vectors.Vec2<double>? CountButtonPresses(Machine machine)
    {
        var aX = machine.MovementA.X;
        var aY = machine.MovementA.Y;
        var bX = machine.MovementB.X;
        var bY = machine.MovementB.Y;
        var pX = machine.PrizeLocation.X;
        var pY = machine.PrizeLocation.Y;

        var a = (bY * pX - bX * pY) / (aX * bY - aY * bX);
        var b = (-aY * pX + aX * pY) / (aX * bY - aY * bX);

        if (!a.IsNaturalNumber() || !b.IsNaturalNumber()) return null;

        return new Vectors.Vec2<double>(a, b);
    }

    private sealed record Machine(
        Vectors.Vec2<double> MovementA,
        Vectors.Vec2<double> MovementB,
        Vectors.Vec2<double> PrizeLocation
    );
}

