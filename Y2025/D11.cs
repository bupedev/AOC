using AOC.Utilities;

namespace AOC.Y2025;

public class D11() : Solution(2025, 11)
{
    protected override object GetPart1Result(string input)
    {
        var connections = ParseDeviceConnections(input);
        
        return CountAllPaths("you", "out", v => connections[v], []);
    }

    protected override object GetPart2Result(string input)
    {
        var connections = ParseDeviceConnections(input);
        
        return CountAllPaths("svr", "out", v => connections[v], ["fft", "dac"]);
    }

    private static IReadOnlyDictionary<string, string[]> ParseDeviceConnections(string input)
    {
        return input
            .SplitOnNewLines()
            .Select(line => line.Split(": "))
            .Select(line => (Device: line[0], Outputs: line[1].Split(' ')))
            .ToDictionary(line => line.Device, line => line.Outputs);
    }

    private static long CountAllPaths(
        string start,
        string end,
        Func<string, string[]> getNext,
        string[] requiredVertices)
    {
        var pathLengthCache = new Dictionary<(string Vertex, string Remaining), long>();
        
        return CountPathLength(start, requiredVertices.ToHashSet());

        long CountPathLength(string vertex, IReadOnlySet<string> remainingRequired)
        {
            if (remainingRequired.Contains(vertex))
            {
                remainingRequired = remainingRequired.Where(required => required != vertex).ToHashSet();
            }

            if (vertex == end)
            {
                return remainingRequired.Count == 0 ? 1 : 0;
            }

            var key = (vertex, BuildRequirementKey(remainingRequired));
            
            if (!pathLengthCache.ContainsKey(key))
            {
                pathLengthCache[key] = getNext(vertex).Sum(next => CountPathLength(next, remainingRequired));
            }

            return pathLengthCache[key];
        }
        
        string BuildRequirementKey(IEnumerable<string> vertices) => string.Join(",", vertices.Order());
    }
}