using Godot;

public partial class CombatZoneUI : Node2D
{
    [Export] private CombatSummonZoneUI CombatSummonZoneUI { get; set; }

    private MonsterCardUI PlayerCard { get; set; }
    private MonsterCardUI EnemyCard { get; set; }

    private GameDataService GameDataService { get; set; }
    private DeskManager DeskManager { get; set; }

    private double timeBeforeNextDraw = 0;
    private bool waitNextDraw = false;

    public override void _Ready()
    {
        GameDataService = GetNode<GameDataService>(GameDataService.Path);
        DeskManager = GetNode<DeskManager>(DeskManager.Path);
        CombatSummonZoneUI.OnReadyToFight += CombatSummonZoneUI_OnReadyToFight;
        DeskManager.OnFightCompleted += DeskManager_OnFightCompleted;
    }

    private void DeskManager_OnFightCompleted(BossFight bossFigth)
    {
        if (PlayerCard == null || EnemyCard == null)
        {
            GD.PrintErr("Player card and enemyCard not supposed to be empty in " + nameof(CombatZoneUI));
            return;
        }

        PlayerCard.QueueFree();
        EnemyCard.QueueFree();
        PlayerCard = null;
        EnemyCard = null;
    }

    private void CombatSummonZoneUI_OnReadyToFight(MonsterCardUI card)
    {
        PlayerCard = card;
        DeskManager.FightStarting(card.SummoningSpecs, EnemyCard.SummoningSpecs);
    }

    public override void _Process(double delta)
    {
        GetNextCard(delta);
    }

    private void GetNextCard(double delta)
    {
        if (EnemyCard != null)
        {
            return;
        }

        var bossQueueManager = BossQueueManager.Instance;
        if (!waitNextDraw && bossQueueManager.IsFullAndWihtouAnimation)
        {
            timeBeforeNextDraw = 0.1f;
            waitNextDraw = true;
        }

        if (waitNextDraw && timeBeforeNextDraw > 0)
        {
            timeBeforeNextDraw -= delta;
        }
        else if (waitNextDraw && timeBeforeNextDraw <= 0)
        {
            timeBeforeNextDraw = 0;
            waitNextDraw = false;
            var firstCard = bossQueueManager.PullNextCard();
            firstCard.TriggerShiftAnim(Position + new Vector2(2, 2));
            EnemyCard = firstCard;
        }
    }
}
