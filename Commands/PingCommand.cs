using System.CommandLine;
using System.Diagnostics;

namespace AOC.Commands;

public static class PingCommand {
    public static Command Build() {
        var command = new Command(
            "ping",
            "Send a request to the server to verify authenticated connection is working correctly."
        );

        command.SetHandler(Ping);

        return command;
    }

    private static void Ping() {
        switch (Client.TryReadPuzzle(2015, 1, out _)) {
            case ClientResponseType.Pass:
                Console.Out.WriteLine("Successfully connected to server with authentication!");
                break;
            case ClientResponseType.Fail:
                Console.Error.WriteLine(
                    "Failed to connect to server with authentication. " +
                    "Ensure that the AOC_SESSION_ID environment variable has been correctly set."
                );
                break;
            case ClientResponseType.Throttle:
                Console.Error.WriteLine(
                    "It has been less than a 150 seconds before last making a request to the server, " +
                    "please wait before trying again!"
                );
                break;
            default:
                throw new UnreachableException();
        }
    }
}
