using System.CommandLine;
using System.CommandLine.Parsing;

namespace AOC.Commands;

public static class Validations {
    public static ValidateSymbolResult<CommandResult> ExactlyOneOf(params Symbol[] symbols) {
        return result => {
            if (result.Children.Count(child => symbols.Contains(child.Symbol)) == 1) return;
            var symbolNames = string.Join(",", symbols.Select(symbol => symbol.Name));
            result.ErrorMessage = $"You must provide exactly one of the following options: {symbolNames}";
        };
    }

    public static ValidateSymbolResult<CommandResult> NoMoreThanOneOf(params Symbol[] symbols) {
        return result => {
            if (result.Children.Count(child => symbols.Contains(child.Symbol)) <= 1) return;
            var symbolNames = string.Join(",", symbols.Select(symbol => symbol.Name));
            result.ErrorMessage = $"You must provide no more than one of the following options: {symbolNames}";
        };
    }
}