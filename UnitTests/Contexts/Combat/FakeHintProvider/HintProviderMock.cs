public class HintProviderMock : IHintProvider
{
    public string GetHintFor(SpecTypes specType, int summonIndex, int bossIndex)
    {
        return $"{specType}_{summonIndex}_{bossIndex}";
    }
}
