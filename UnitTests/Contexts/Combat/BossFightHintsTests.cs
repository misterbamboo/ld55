using Xunit;

namespace UnitTests.Contexts.Combat;

public class BossFightHintsTests
{

    [Fact]
    public void Combat_hint_for_win_emotion()
    {
        // Arrange
        var bossFight = new BossFight(/* TODO: Mock IHintProvider*/);
        var summonSpecs = new SummoningSpecs(1, 0, 0);
        var bossSpecs = new SummoningSpecs(2, 0, 0);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal("Win because emotion 1 is better than emotion 2", bossFight.Hint);
    }
}