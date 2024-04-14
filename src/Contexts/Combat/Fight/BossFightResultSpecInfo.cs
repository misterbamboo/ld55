public class BossFightResultSpecInfo
{
    public BossFightResultSpecInfo(BossFight.BossFightResult bossFightResult, int summonIndex, int bossIndex, SpecTypes specType)
    {
        BossFightResult = bossFightResult;
        SummonIndex = summonIndex;
        BossIndex = bossIndex;
        SpecType = specType;
    }

    public BossFight.BossFightResult BossFightResult { get; }
    public int SummonIndex { get; }
    public int BossIndex { get; }
    public SpecTypes SpecType { get; }
}
