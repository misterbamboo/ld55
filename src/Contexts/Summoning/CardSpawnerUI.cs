using Godot;
using System;

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
        var newMonsterCard = MonsterCardUIPrefab.Instantiate<MonsterCardUI>();

        var emotionSpecDef = gameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Emotion, summoningSpecs.Emotion.Index));
        var elementSpecDef = gameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Element, summoningSpecs.Element.Index));
        var speciesSpecDef = gameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Species, summoningSpecs.Species.Index));

        newMonsterCard.IsDraggable = true;
        newMonsterCard.Init(summoningSpecs, emotionSpecDef, elementSpecDef, speciesSpecDef);
       
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
