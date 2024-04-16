using Godot;

public partial class CardSpawnerUI : Node2D
{
	private DeskManager deskManager;
    private GameDataService gameDataService;
    [Export] private PackedScene MonsterCardUIPrefab;
	private Node2D spawnTarget;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		deskManager = GetNode<DeskManager>(DeskManager.Path);
        gameDataService = GetNode<GameDataService>(GameDataService.Path);
		spawnTarget = GetNode<Node2D>("SpawnTarget");
        deskManager.OnMonsterSummoned += SpawnCard;
	}

    private void SpawnCard(SummoningSpecs summoningSpecs)
    {
        GD.Print("Spawning card");
        GD.Print(summoningSpecs);
        var newMonsterCard = MonsterCardUIPrefab.Instantiate<MonsterCardUI>();

        newMonsterCard.IsDraggable = true;
        newMonsterCard.Init(summoningSpecs);
       
        newMonsterCard.Position = spawnTarget.Position;

        AddChild(newMonsterCard);
        MoveChild(newMonsterCard, 0);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        deskManager.OnMonsterSummoned -= SpawnCard;
    }
}
