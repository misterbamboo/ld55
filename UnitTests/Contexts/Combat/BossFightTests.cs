using Xunit;

namespace UnitTests.Contexts.Combat;

public class BossFightTests
{
    private HintProviderMock hintProviderMock;

    public BossFightTests()
    {
        hintProviderMock = new HintProviderMock();
    }

    [Fact]
    public void Create_BossFight()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);

        // Act

        // Assert
        Assert.True(bossFight != null);
    }

    [Fact]
    public void Combat_result_draw()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(1, 1, 1);
        var bossSpecs = new SummoningSpecs(1, 1, 1);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.Draw, bossFight.Result);
    }

    [Fact]
    public void Combat_result_draw_when_1_win_1lose_1draw()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(1, 2, 0);
        var bossSpecs = new SummoningSpecs(2, 1, 0);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.Draw, bossFight.Result);
    }

    [Fact]
    public void Combat_result_lose_on_2_specs()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(2, 2, 0);
        var bossSpecs = new SummoningSpecs(1, 1, 0);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.PlayerLose, bossFight.Result);
    }

    [Fact]
    public void Combat_result_lose_on_3_specs()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(2, 2, 2);
        var bossSpecs = new SummoningSpecs(1, 1, 1);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.PlayerLose, bossFight.Result);
    }

    [Fact]
    public void Combat_result_win_on_1_specs()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(1, 0, 0);
        var bossSpecs = new SummoningSpecs(3, 0, 0);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.PlayerWin, bossFight.Result);
    }

    [Fact]
    public void Combat_result_win_on_2_specs()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(1, 1, 0);
        var bossSpecs = new SummoningSpecs(3, 3, 0);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.PlayerWin, bossFight.Result);
    }

    [Fact]
    public void Combat_result_win_on_2_specs_and_lose_on_1()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(1, 1, 2);
        var bossSpecs = new SummoningSpecs(3, 3, 0);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.PlayerWin, bossFight.Result);
    }

    [Fact]
    public void Combat_result_win_on_3_specs()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(1, 1, 1);
        var bossSpecs = new SummoningSpecs(3, 3, 3);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.PlayerWin, bossFight.Result);
    }
}