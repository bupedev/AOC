using AOC.Utilities;

namespace AOC.Y2015;

public class D01NotQuiteLisp() : Solution(2015, 1) {
    protected override object GetPart1Result(string input) {
        return input.Aggregate(
            0,
            (floor, character) => character switch {
                '(' => floor + 1,
                ')' => floor - 1,
                _ => throw new InvalidOperationException()
            }
        );
    }

    protected override object GetPart2Result(string input) {
        return input
            .WithIndices()
            .Aggregate(
                (Floor: 0, BasementIndex: 0),
                (state, character) => {
                    if (state.BasementIndex > 0) return state;
                    return character.Value switch {
                        '(' => (state.Floor + 1, state.BasementIndex),
                        ')' => (state.Floor - 1, state.Floor is 0 ? character.Index + 1 : state.BasementIndex),
                        _ => throw new InvalidOperationException()
                    };
                }
            )
            .BasementIndex;
    }
}