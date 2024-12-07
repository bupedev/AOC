using System.Globalization;
using System.Net;

namespace AOC;

public enum ClientResponseType {
    Pass,
    Fail,
    Throttle
}

public static class Client {
    private static readonly TimeSpan ThrottleRate = new(0, 2, 30);

    private static readonly HttpClient HttpClient = new() {
        BaseAddress = new Uri("https://adventofcode.com/"),
        DefaultRequestHeaders = {
            { "Cookie", $"session={Environment.GetEnvironmentVariable("AOC_SESSION_ID")}" },
            { "User-Agent", WebUtility.UrlEncode("https://github.com/bupedev/AOC by ben@bupe.dev") }
        }
    };

    public static ClientResponseType TryReadPropertiesFromServer(int year, int day, out string puzzleInput) {
        puzzleInput = string.Empty;

        if (ThrottleRequired()) return ClientResponseType.Throttle;

        var result = Get($"{year}/day/{day}/input");

        if (result.StatusCode == HttpStatusCode.OK) {
            puzzleInput = result.Content.ReadAsStringAsync().Result;
            return ClientResponseType.Pass;
        }

        return ClientResponseType.Fail;
    }

    private static HttpResponseMessage Get(string requestUri) {
        Environment.SetEnvironmentVariable(
            "AOC_LAST_REQUEST",
            DateTime.UtcNow.ToString("o"),
            EnvironmentVariableTarget.User
        );
        return HttpClient.Send(new HttpRequestMessage(HttpMethod.Get, requestUri));
    }

    private static bool ThrottleRequired() {
        if (Environment.GetEnvironmentVariable("AOC_LAST_REQUEST", EnvironmentVariableTarget.User) is
            not { } lastRequestString)
            return false;

        var lastRequest = DateTime.Parse(lastRequestString, null, DateTimeStyles.RoundtripKind);
        var timeSinceLastRequest = DateTime.UtcNow - lastRequest;

        return timeSinceLastRequest < ThrottleRate;
    }
}