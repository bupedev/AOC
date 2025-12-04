using System.Collections.Immutable;
using System.Globalization;
using System.Net;

namespace AOC;

public enum ClientResponseType {
    Pass,
    Fail,
    Throttle
}

public enum RequestKind
{
    Puzzle,
    Leaderboard
}

public static class Client {
    private static readonly ImmutableDictionary<RequestKind, TimeSpan> ThrottleRate = new Dictionary<RequestKind, TimeSpan> {[RequestKind.Puzzle] = new(0, 2, 30), [RequestKind.Leaderboard] = new(0, 15, 0)}.ToImmutableDictionary();

    private static readonly HttpClient HttpClient = new() {
        BaseAddress = new Uri("https://adventofcode.com/"),
        DefaultRequestHeaders = {
            { "Cookie", $"session={Environment.GetEnvironmentVariable("AOC_SESSION_ID")}" },
            { "User-Agent", WebUtility.UrlEncode("https://github.com/bupedev/AOC by ben@bupe.dev") }
        }
    };

    public static ClientResponseType TryReadPuzzle(int year, int day, out string puzzleInput) {
        return TryRead(
            RequestKind.Leaderboard, 
            $"{year}/day/{day}/input", 
            out puzzleInput);
    }
    
    public static ClientResponseType TryReadLeaderboard(int year, int leaderboardId, out string puzzleInput)
    {
        return TryRead(
            RequestKind.Leaderboard, 
            $"{year}/leaderboard/private/view/{leaderboardId}.json", 
            out puzzleInput);
    }
    
    private static ClientResponseType TryRead(RequestKind kind, string requestUri, out string response) {
        response = string.Empty;

        if (ThrottleRequired(kind)) return ClientResponseType.Throttle;

        var result = Get(kind, requestUri);

        if (result.StatusCode == HttpStatusCode.OK) {
            response = result.Content.ReadAsStringAsync().Result;
            return ClientResponseType.Pass;
        }

        return ClientResponseType.Fail;
    }

    private static HttpResponseMessage Get(RequestKind kind, string requestUri) {
        Environment.SetEnvironmentVariable(
            LastRequestVariable(kind),
            DateTime.UtcNow.ToString("o"),
            EnvironmentVariableTarget.User
        );
        return HttpClient.Send(new HttpRequestMessage(HttpMethod.Get, requestUri));
    }

    private static bool ThrottleRequired(RequestKind kind) {
        if (Environment.GetEnvironmentVariable(LastRequestVariable(kind), EnvironmentVariableTarget.User) is
            not { } lastRequestString)
            return false;

        var lastRequest = DateTime.Parse(lastRequestString, null, DateTimeStyles.RoundtripKind);
        var timeSinceLastRequest = DateTime.UtcNow - lastRequest;

        return timeSinceLastRequest < ThrottleRate[kind];
    }
    
    private static string LastRequestVariable(RequestKind kind) => $"AOC_LAST_{kind.ToString().ToUpper()}_REQUEST";
}