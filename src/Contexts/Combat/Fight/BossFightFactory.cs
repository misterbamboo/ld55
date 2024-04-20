public class BossFightFactory
{
    private IHintProvider hintProvider;

    public BossFightFactory(IHintProvider hintProvider)
    {
        this.hintProvider = hintProvider;
    }

    internal BossFight Combat(SummoningSpecs player, SummoningSpecs enemy)
    {
        var bossFight = new BossFight(hintProvider, player, enemy);
        bossFight.Combat();
        return bossFight;
    }
}
