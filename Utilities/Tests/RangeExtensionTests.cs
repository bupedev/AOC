using FluentAssertions;

namespace AOC.Utilities.Tests;

public class RangeExtensionTests {
    [Test]
    public void Split_BeforeStart_ProducesCorrectRanges() {
        (..10).Split(-1).Should().BeEquivalentTo([(..10)]);
    }

    [Test]
    public void Split_AtStart_ProducesCorrectRanges() {
        (..10).Split(0).Should().BeEquivalentTo([(..10)]);
    }

    [Test]
    public void Split_AfterStart_ProducesCorrectRanges() {
        (..10).Split(1).Should().BeEquivalentTo([(..1), 1..10]);
    }

    [Test]
    public void Split_BeforeEnd_ProducesCorrectRanges() {
        (..10).Split(9).Should().BeEquivalentTo([(..9), 9..10]);
    }

    [Test]
    public void Split_AtEnd_ProducesCorrectRanges() {
        (..10).Split(10).Should().BeEquivalentTo([(..10)]);
    }

    [Test]
    public void Split_AfterEnd_ProducesCorrectRanges() {
        (..10).Split(11).Should().BeEquivalentTo([(..10)]);
    }
}