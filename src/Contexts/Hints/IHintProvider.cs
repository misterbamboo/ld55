public interface IHintProvider
{
    string GetHintFor(SpecTypes specType, int summonIndex, int bossIndex);
}
