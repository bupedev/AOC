using static AOC.Utilities.Miscellaneous;

namespace AOC.Y2024;

public class D05PrintQueue() : Solution(2024, 5) {
    protected override object GetPart1Result(string input) {
        return Parse(input)
            .Then(
                parsed => parsed
                    .Lists
                    .Where(IsInOrder(parsed.Dependencies))
                    .Select(list => list[list.Count / 2])
                    .Sum()
            );
    }

    protected override object GetPart2Result(string input) {
        return Parse(input)
            .Then(
                parsed => parsed
                    .Lists
                    .Where(Not(IsInOrder(parsed.Dependencies)))
                    .Select(OrderList(parsed.Dependencies))
                    .Select(list => list[list.Count / 2])
                    .Sum()
            );
    }

    private static (ILookup<int, int> Dependencies, IReadOnlyList<int[]> Lists) Parse(string input) {
        var parts = input.Split(Environment.NewLine + Environment.NewLine);
        return (
            Dependencies: parts[0]
                .SplitOnNewLines()
                .Select(line => line.Split("|").Select(int.Parse).ToArray())
                .ToLookup(p => p[1], p => p[0]),
            Lists: parts[1].SplitOnNewLines().Select(line => line.Split(",").Select(int.Parse).ToArray()).ToArray()
        );
    }

    private static Func<IReadOnlyList<int>, bool> IsInOrder(ILookup<int, int> dependencies) {
        return list => list.Aggregate(
                (PredecessorSet: new HashSet<int>(), InOrder: true),
                (state, value) => {
                    if (!state.InOrder) return state;
                    var inOrder = !state.PredecessorSet.Contains(value);
                    foreach (var predecessors in dependencies[value]) state.PredecessorSet.Add(predecessors);
                    return state with {
                        InOrder = inOrder
                    };
                }
            )
            .InOrder;
    }

    private static Func<IReadOnlyList<int>, IReadOnlyList<int>> OrderList(ILookup<int, int> map) {
        return list => {
            var ordered = new List<int>();
            var predecessorMap = list.ToDictionary(elem => elem, elem => map[elem].Intersect(list).ToHashSet());
            while (predecessorMap.Count != 0) {
                ordered.AddRange(predecessorMap.Where(kvp => kvp.Value.Count == 0).Select(kvp => kvp.Key));
                foreach (var value in ordered) predecessorMap.Remove(value);
                foreach (var valueSet in predecessorMap.Values) valueSet.ExceptWith(ordered);
            }

            return ordered.ToArray();
        };
    }
}