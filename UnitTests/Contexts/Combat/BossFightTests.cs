using Xunit;

namespace UnitTests.Contexts.Combat;

public class BossFightTests
{
    private const int Emo_0_Sadness = 0;
    private const int Emo_1_Fear = 1;
    private const int Emo_2_Anger = 2;
    private const int Emo_3_Disgust = 3;
    private const int Emo_4_Joy = 4;

    private const int Ele_0_Water = 0;
    private const int Ele_1_Ice = 1;
    private const int Ele_2_Lightning = 2;
    private const int Ele_3_Fire = 3;
    private const int Ele_4_Earth = 4;

    private const int Spe_0_Undead = 0;
    private const int Spe_1_Ghost = 1;
    private const int Spe_2_Fairy = 2;
    private const int Spe_3_Slime = 3;
    private const int Spe_4_Golem = 4;
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
        var summonSpecs = new SummoningSpecs(Emo_1_Fear, Ele_1_Ice, Spe_1_Ghost);
        var bossSpecs = new SummoningSpecs(Emo_1_Fear, Ele_1_Ice, Spe_1_Ghost);

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
        var summonSpecs = new SummoningSpecs(Emo_2_Anger, Ele_1_Ice, Spe_0_Undead);
        var bossSpecs = new SummoningSpecs(Emo_1_Fear, Ele_2_Lightning, Spe_0_Undead);

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
        var summonSpecs = new SummoningSpecs(Emo_1_Fear, Ele_1_Ice, Spe_0_Undead);
        var bossSpecs = new SummoningSpecs(Emo_2_Anger, Ele_2_Lightning, Spe_0_Undead);

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
        var summonSpecs = new SummoningSpecs(Emo_1_Fear, Ele_1_Ice, Spe_1_Ghost);
        var bossSpecs = new SummoningSpecs(Emo_2_Anger, Ele_2_Lightning, Spe_2_Fairy);

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
        var summonSpecs = new SummoningSpecs(Emo_3_Disgust, Ele_0_Water, Spe_0_Undead);
        var bossSpecs = new SummoningSpecs(Emo_1_Fear, Ele_0_Water, Spe_0_Undead);

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
        var summonSpecs = new SummoningSpecs(Emo_3_Disgust, Ele_3_Fire, Spe_0_Undead);
        var bossSpecs = new SummoningSpecs(Emo_1_Fear, Ele_1_Ice, Spe_0_Undead);

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
        var summonSpecs = new SummoningSpecs(Emo_3_Disgust, Ele_3_Fire, Spe_0_Undead);
        var bossSpecs = new SummoningSpecs(Emo_1_Fear, Ele_1_Ice, Spe_2_Fairy);

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
        var summonSpecs = new SummoningSpecs(Emo_3_Disgust, Ele_3_Fire, Spe_3_Slime);
        var bossSpecs = new SummoningSpecs(Emo_1_Fear, Ele_1_Ice, Spe_1_Ghost);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.PlayerWin, bossFight.Result);
    }

    [Fact]
    public void Combat_golem_fire_joy_shouldwin_agains_golem_lightning_joy()
    {
        // Arrange
        var bossFight = new BossFight(hintProviderMock);
        var summonSpecs = new SummoningSpecs(Emo_4_Joy, Ele_3_Fire, Spe_4_Golem);
        var bossSpecs = new SummoningSpecs(Emo_4_Joy, Ele_2_Lightning, Spe_4_Golem);

        // Act
        bossFight.Combat(summonSpecs, bossSpecs);

        // Assert
        Assert.Equal(BossFight.BossFightResult.PlayerWin, bossFight.Result);
    }
}