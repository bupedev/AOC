using AOC.Utilities;
using static System.String;

namespace AOC.Y2025;

public class D03() : Solution(2025, 3)
{
    protected override object GetPart1Result(string input)
    {
        return input.SplitOnNewLines().Select(battery => GetMaxJoltage(battery, 2)).Sum();
    }

    protected override object GetPart2Result(string input)
    {
        return input.SplitOnNewLines().Select(battery => GetMaxJoltage(battery, 12)).Sum();
    }
    
    private static long GetMaxJoltage(string arg, int joltageLength)
    {
        var batteryDigits = arg.Select(c => c - '0').ToList();
        
        var joltageDigits = new List<long>();
        var rollingOffset = 0;
        
        for (int digitBuffer = joltageLength; digitBuffer > 1; digitBuffer--)
        {
            var digitCandidates = batteryDigits[rollingOffset..^(digitBuffer-1)]
                .Index()
                .GroupBy(x => x.Item)
                .MaxBy(g => g.Key);
            
            joltageDigits.Add(digitCandidates!.Key);
            rollingOffset += digitCandidates.MaxBy(c => batteryDigits[(c.Index+1)..^(digitBuffer - 2)].Max()).Index + 1;
        }
        
        joltageDigits.Add(batteryDigits[rollingOffset..].Max());

        return long.Parse(Join(Empty, joltageDigits));
    }
}