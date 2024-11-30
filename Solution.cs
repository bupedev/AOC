namespace AOC;

/// <summary>
/// A solution for the puzzle on a particular day of a particular year.
/// </summary>
/// <param name="yearNumber"> The year with which the solution is associated. </param>
/// <param name="dayNumber"> The day of December with which the solution is associated. </param>
/// <remarks> The combination of year and day should be distinct and unique for each extension of this class. </remarks>
public abstract class Solution(int yearNumber, int dayNumber) {
    /// <summary>
    /// A dictionary containing all solutions in the project, keyed by their year and day numbers.
    /// </summary>
    public static readonly Dictionary<(int, int), Solution> ByDay = typeof(Solution)
        .Assembly
        .GetTypes()
        .Where(type => type.IsSubclassOf(typeof(Solution)))
        .Select(Activator.CreateInstance)
        .Cast<Solution>()
        .ToDictionary(s => (s._yearNumber, s._dayNumber));

    private readonly int _dayNumber = dayNumber;

    private readonly int _yearNumber = yearNumber;

    /// <summary>
    /// Get the solution for the first part of the puzzle.
    /// </summary>
    /// <param name="input"> The input text for the puzzle. </param>
    /// <returns> An object containing the solution result. This is left untyped for flexibility. </returns>
    protected abstract object GetPart1Result(string input);

    /// <summary>
    /// Get the solution for the second part of the puzzle.
    /// </summary>
    /// <param name="input"> The input text for the puzzle. </param>
    /// <returns> An object containing the solution result. This is left untyped for flexibility. </returns>
    protected abstract object GetPart2Result(string input);

    /// <summary>
    /// Get the solution functions for all parts of the puzzle with their part number.
    /// </summary>
    public IEnumerable<(int PartNumber, Func<string, object> SolutionFunction)> GetFunctionsForAllParts() {
        yield return (1, GetPart1Result);
        yield return (2, GetPart2Result);
    }
}