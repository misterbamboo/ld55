﻿using Xunit;

namespace UnitTests.Contexts.Combat;

public class BossFightHintsTests
{

    [Fact]
    public void Combat_hint_for_win_emotion()
    {
        // Arrange
        var hintProviderMock = new HintProviderMock();
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(1, 0, 0);
        var bossSpecs = new SummoningSpecs(2, 0, 0);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal("Emotion_1_2", bossFight.Hint);
    }
}