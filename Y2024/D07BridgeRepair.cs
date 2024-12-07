using AOC.Utilities;

namespace AOC.Y2024;

public class D07() : Solution(2024, 7) {
    private static readonly Dictionary<int, List<Operator[]>> OperatorPermutationCache = new();

    protected override object GetPart1Result(string input) {
        return input
            .SplitOnNewLines()
            .Select(Parse)
            .Where(eq => IsEquationPossible(eq, [Operator.Multiply, Operator.Add]))
            .Sum(eq => eq.Result);
    }

    protected override object GetPart2Result(string input) {
        return input
            .SplitOnNewLines()
            .Select(Parse)
            .Where(equation => IsEquationPossible(equation, [Operator.Multiply, Operator.Add, Operator.Concat]))
            .Sum(equation => equation.Result);
    }

    private bool IsEquationPossible(EquationCandidate equationCandidate, Operator[] operators) {
        var operatorCount = equationCandidate.Operands.Length - 1;
        if (!OperatorPermutationCache.TryGetValue(operatorCount, out var operatorPermutations)) {
            operatorPermutations = operators.GetAllPermutations(operatorCount).ToList();
            OperatorPermutationCache.Add(operatorCount, operatorPermutations);
        }

        return operatorPermutations.Any(
            permutation => {
                var actualResult = equationCandidate
                    .Operands
                    .Skip(1)
                    .Zip(permutation, (operand, op) => (Operand: operand, Operator: op))
                    .Aggregate(
                        equationCandidate.Operands[0],
                        (agg, t) => t.Operator switch {
                            Operator.Multiply => agg * t.Operand,
                            Operator.Add => agg + t.Operand,
                            Operator.Concat => long.Parse(agg.ToString() + t.Operand),
                            _ => throw new ArgumentOutOfRangeException()
                        }
                    );
                return actualResult == equationCandidate.Result;
            }
        );
    }

    private static EquationCandidate Parse(string line) {
        var colonSplit = line.Split(":");
        var result = long.Parse(colonSplit[0]);
        var operands = colonSplit[1].SplitOnWhitespace().Select(long.Parse).ToArray();

        return new EquationCandidate(result, operands);
    }

    private enum Operator {
        Multiply,
        Add,
        Concat
    }

    private sealed record EquationCandidate(long Result, long[] Operands);
}