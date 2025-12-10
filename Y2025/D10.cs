using AOC.Utilities;
using Microsoft.Z3;

namespace AOC.Y2025;

public class D10() : Solution(2025, 10)
{
    protected override object GetPart1Result(string input)
    {
        var machines = input.SplitOnNewLines().Select(line => line.Split(' ')).Select(line =>
        {
            var goal = PatternToBitmask(line[0][1..^1]);
            var buttons = line[1..^1]
                .Select(schematic => schematic[1..^1].Split(',').Select(int.Parse).ToArray())
                .Select(IndicesToBitmask)
                .ToArray();

            return new PatternMachine(goal, buttons);
        });

        return machines.Sum(CalculateMinimumPresses);
    }

    private static int CalculateMinimumPresses(PatternMachine machine)
    {
        var permutationCount = 1;
        while (true)
        {
            foreach (var sequence in machine.Buttons.GetAllPermutations(permutationCount))
            {
                var field = 0;
                foreach (var button in sequence)
                {
                    field ^= button;
                }

                if (field == machine.Goal) return permutationCount;
            }

            permutationCount++;
        }
    }

    private sealed record PatternMachine(int Goal, int[] Buttons);

    private static int PatternToBitmask(string pattern)
    {
        int result = 0;

        foreach (var (index, character) in pattern.Reverse().Index())
        {
            if (character == '#')
            {
                result |= 1 << (pattern.Length - 1 - index);
            }
        }

        return result;
    }

    private static int IndicesToBitmask(IEnumerable<int> indices)
    {
        int result = 0;

        foreach (var index in indices)
        {
            result |= 1 << index;
        }

        return result;
    }

    protected override object GetPart2Result(string input)
    {
        var machines = input.SplitOnNewLines().Select(line => line.Split(' ')).Select(line =>
        {
            var goal = line[^1][1..^1].Split(',').Select(int.Parse).ToArray();
            var buttons = line[1..^1]
                .Select(schematic => schematic[1..^1].Split(',').Select(int.Parse).ToArray())
                .ToArray();

            return new JoltageMachine(goal, buttons);
        });

        return machines.Sum(CalculateMinimumPresses);
    }

    private sealed record JoltageMachine(int[] Goal, int[][] Buttons);

    private static int CalculateMinimumPresses(JoltageMachine machine)
    {
        using var z3 = new Context();

        // Integer variable for press counts of each button
        var pressCounts = (..machine.Buttons.Length).Iterate()
            .Select(i => z3.MkIntConst($"press_count{i}"))
            .ToArray<ArithExpr>();

        // Formulate optimization model
        using var optimization = z3.MkOptimize();

        // Constraint (requirement) for goal (joltage sums)
        var joltageSumConstraints = machine.Goal.Index()
            .Select(joltage =>
            {
                return (Joltage: joltage, Buttons: pressCounts
                    .Where((_, buttonIndex) => machine.Buttons[buttonIndex].Contains(joltage.Index))
                    .ToArray());
            })
            .Where(t => t.Buttons.Any())
            .Select(t => z3.MkEq(z3.MkAdd(t.Buttons), z3.MkInt(t.Joltage.Item)))
            .ToArray();
        optimization.Add(joltageSumConstraints);

        // Constraints for press count variables (count >= 0)
        var pressCountConstraints = pressCounts.Select(pressCount => z3.MkGe(pressCount, z3.MkInt(0))).ToArray();
        optimization.Add(pressCountConstraints);

        // Objective: minimize press count
        optimization.MkMinimize(pressCounts.Length == 1 ? pressCounts[0] : z3.MkAdd(pressCounts));

        // Check and solve
        optimization.Check();
        var result = optimization.Model;

        return pressCounts.Sum(pressCount => ((IntNum)result.Evaluate(pressCount, true)).Int);
    }
}