using AOC.Utilities;

namespace AOC.Y2021;

public class D02Dive() : Solution(2021, 2) {
    protected override object GetPart1Result(string input) {
        var finalState = input
            .SplitOnNewLines()
            .Select(ParseLine)
            .Aggregate(
                (Depth: 0, Distance: 0),
                (state, command) => {
                    return command.Axis switch {
                        Axis.Horizontal => state with {
                            Distance = state.Distance + command.Quantity
                        },
                        Axis.Vertical => state with {
                            Depth = state.Depth + command.Quantity
                        },
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            );

        return finalState.Depth * finalState.Distance;
    }

    protected override object GetPart2Result(string input) {
        var finalState = input
            .SplitOnNewLines()
            .Select(ParseLine)
            .Aggregate(
                (Aim: 0, Depth: 0, Distance: 0),
                (state, command) => command.Axis switch {
                    Axis.Vertical => state with {
                        Aim = state.Aim + command.Quantity
                    },
                    Axis.Horizontal => state with {
                        Distance = state.Distance + command.Quantity,
                        Depth = state.Depth + state.Aim * command.Quantity
                    },
                    _ => throw new ArgumentOutOfRangeException()
                }
            );

        return finalState.Depth * finalState.Distance;
    }

    private static (Axis Axis, int Quantity) ParseLine(string line) {
        var tokens = line.SplitOnWhitespace();

        var axis = tokens[0] switch {
            "forward" => Axis.Horizontal,
            "up" or "down" => Axis.Vertical,
            _ => throw null!
        };

        var quantity = int.Parse(tokens[1]) * (tokens[0] == "up" ? -1 : 1);

        return (Axis: axis, Quantity: quantity);
    }

    private enum Axis {
        Horizontal,
        Vertical
    }
}