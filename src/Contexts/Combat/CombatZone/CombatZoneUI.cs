using Godot;

public partial class CombatZoneUI : Node2D
{
    [Export] private CombatSummonZoneUI CombatSummonZoneUI { get; set; }

    private double PullFirstBossInSeconds = 1;
    public double startedTime;
    public bool FistCardPulled { get; set; }

    private MonsterCardUI EnemyCard { get; set; }

    private GameDataService GameDataService { get; set; }

    public override void _Ready()
    {
        GameDataService = GetNode<GameDataService>(GameDataService.Path);
        CombatSummonZoneUI.OnReadyToFight += CombatSummonZoneUI_OnReadyToFight;
    }

    private void CombatSummonZoneUI_OnReadyToFight(MonsterCardUI card)
    {
        var bossFight = new BossFight(GameDataService);
        bossFight.Combat(card.SummoningSpecs, EnemyCard.SummoningSpecs);
    }

    public override void _Process(double delta)
    {
        CheckPullFirstCard(delta);
    }

    private void CheckPullFirstCard(double delta)
    {
        if (FistCardPulled)
        {
            return;
        }

        startedTime += delta;
        if (startedTime > PullFirstBossInSeconds)
        {
            var firstCard = BossQueueManager.Instance.PullNextCard();
            firstCard.TriggerShiftAnim(Position + new Vector2(2, 2));
            EnemyCard = firstCard;
            FistCardPulled = true;
        }
    }
}
