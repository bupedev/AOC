using AOC.Utilities;
using AOC.Utilities.Grids;

namespace AOC.Y2024;

public class D08ResonantCollinearity() : Solution(2024, 8) {
    protected override object GetPart1Result(string input) {
        return input
            .ToGrid()
            .Then(
                map => {
                    return map
                        .GetAntennaGroups()
                        .FindAllAntinodes(
                            antennae => antennae
                                .GetUniquePairs()
                                .SelectMany(
                                    pair => {
                                        var diff = pair.Second - pair.First;
                                        return new[] { pair.First - diff, pair.Second + diff };
                                    }
                                )
                                .Where(map.InRange)
                        )
                        .Count();
                }
            );
    }

    protected override object GetPart2Result(string input) {
        return input
            .ToGrid()
            .Then(
                map => {
                    return map
                        .GetAntennaGroups()
                        .FindAllAntinodes(
                            antennae => antennae
                                .GetUniquePairs()
                                .SelectMany(
                                    pair => {
                                        var antinodes = new List<GridIndex>();

                                        var diff = pair.Second - pair.First;

                                        var forward = pair.First - diff;
                                        while (map.InRange(forward)) {
                                            antinodes.Add(forward);
                                            forward -= diff;
                                        }

                                        var backwards = pair.Second + diff;
                                        while (map.InRange(backwards)) {
                                            antinodes.Add(backwards);
                                            backwards += diff;
                                        }

                                        return antinodes;
                                    }
                                )
                                .Concat(antennae)
                                .Where(map.InRange)
                        )
                        .Count();
                }
            );
    }
}

public static class D08Utilities {
    public static IEnumerable<GridIndex> FindAllAntinodes(
        this IEnumerable<IGrouping<char, GridIndex>> antennaGroups,
        Func<IEnumerable<GridIndex>, IEnumerable<GridIndex>> getAntinodesFromAntennas
    ) {
        return antennaGroups
            .Aggregate(
                new HashSet<GridIndex>(),
                (set, antennaGroup) => set
                    .Union(getAntinodesFromAntennas(antennaGroup))
                    .ToHashSet()
            );
    }

    public static IEnumerable<IGrouping<char, GridIndex>> GetAntennaGroups(this Grid<char> map) {
        return map
            .FindIndices(tile => tile != '.')
            .ToLookup(antennaIndex => map[antennaIndex], antennaIndex => antennaIndex);
    }
}