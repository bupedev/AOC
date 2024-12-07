using AOC.Utilities.Grids;
using FluentAssertions;

namespace AOC.Utilities.Tests.Grids;

[TestFixture]
public class DirectionsTests {
    [Test]
    [TestCase(CompassDirection.North, TurnDirection.Right, CompassDirection.East)]
    [TestCase(CompassDirection.East, TurnDirection.Right, CompassDirection.South)]
    [TestCase(CompassDirection.South, TurnDirection.Right, CompassDirection.West)]
    [TestCase(CompassDirection.West, TurnDirection.Right, CompassDirection.North)]
    [TestCase(CompassDirection.North, TurnDirection.Left, CompassDirection.West)]
    [TestCase(CompassDirection.East, TurnDirection.Left, CompassDirection.North)]
    [TestCase(CompassDirection.South, TurnDirection.Left, CompassDirection.East)]
    [TestCase(CompassDirection.West, TurnDirection.Left, CompassDirection.South)]
    [TestCase(CompassDirection.North, TurnDirection.Forward, CompassDirection.North)]
    [TestCase(CompassDirection.East, TurnDirection.Forward, CompassDirection.East)]
    [TestCase(CompassDirection.South, TurnDirection.Forward, CompassDirection.South)]
    [TestCase(CompassDirection.West, TurnDirection.Forward, CompassDirection.West)]
    [TestCase(CompassDirection.North, TurnDirection.Backward, CompassDirection.South)]
    [TestCase(CompassDirection.East, TurnDirection.Backward, CompassDirection.West)]
    [TestCase(CompassDirection.South, TurnDirection.Backward, CompassDirection.North)]
    [TestCase(CompassDirection.West, TurnDirection.Backward, CompassDirection.East)]
    public void Turn_AdjustsCompassDirectionCorrectly(
        CompassDirection compassDirection,
        TurnDirection turnDirection,
        CompassDirection expectedResultingCompassDirection
    ) {
        // Act
        var actualResultingCompassDirection = compassDirection.Turn(turnDirection);

        // Assert
        actualResultingCompassDirection.Should().Be(expectedResultingCompassDirection);
    }
}