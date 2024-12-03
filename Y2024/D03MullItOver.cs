using System.Text.RegularExpressions;

namespace AOC;

public partial class D03MullItOver() : Solution(2024, 3) {
    [GeneratedRegex(@"mul\((\d+)\,(\d+)\)")]
    private static partial Regex Part1Regex();
    
    protected override object GetPart1Result(string input)
    {
        return Part1Regex().Matches(input)
            .Select(m => m.Groups.Values.ToArray()[1..].Select(x => x.ToString()).Select(int.Parse).ToArray())
            .Aggregate(0, (sum, values) => sum + values[0] * values[1]);
    }
    
    [GeneratedRegex(@"mul\((\d+)\,(\d+)\)|do\(\)|don\'t\(\)")]
    private static partial Regex Part2Regex();

    protected override object GetPart2Result(string input)
    {
        return Part2Regex().Matches(input)
            .Select(m => m.Groups.Values.ToArray())
            .Select(t =>
            {
                var instructionText = t[0].ToString().Split("(")[0];
                return instructionText switch
                {
                    "mul" => Instruction.Mul(t[1..].Select(x => x.ToString()).Select(int.Parse).ToArray()),
                    "do" => Instruction.Do,
                    "don't" => Instruction.Dont,
                    _ => throw new InvalidOperationException()
                };
            }).Aggregate(new EvaluationState(true, 0), (state, instruction) =>
            {
                return instruction.Type switch
                {
                    InstructionType.Mul => state.Enabled switch
                    {
                        true => state with {Value = state.Value + instruction.Values[0] * instruction.Values[1]},
                        false => state with {}
                    },
                    InstructionType.Do => state with {Enabled = true},
                    InstructionType.Dont => state with {Enabled = false},
                    _ => throw new InvalidOperationException()
                };
            }).Value;
    }

    private enum InstructionType { Mul, Do, Dont }

    private sealed record Instruction(InstructionType Type, int[] Values)
    {
        public static Instruction Do => new(InstructionType.Do, []);
        public static Instruction Dont => new(InstructionType.Dont, []);
        public static Instruction Mul(int[] values) => new(InstructionType.Mul, values);
    }

    private sealed record EvaluationState(bool Enabled, int Value);
}