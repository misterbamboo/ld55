using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class BossQueueManager : Node
{

    public const int MaxInQueue = 5;
    public static BossQueueManager Instance { get; private set; }

    private const double SpawnRateInSecs = 0.5;

    private double timeBeforeNextSpawn;

    private RandomNumberGenerator rand;

    [Export] private Node CardContainer { get; set; }

    [Export] private PackedScene MonsterCardUIPrefab { get; set; }

    private Queue<MonsterCardUI> MonsterQueue { get; set; } = new Queue<MonsterCardUI>();
    public bool IsFullAndWihtouAnimation => MonsterQueue.Where(c => !c.IsAnimating).Count() >= MaxInQueue;

    private Vector2I ScreenSize { get; set; }

    private GameDataService GameDataService { get; set; }

    internal MonsterCardUI PullNextCard()
    {
        if (!MonsterQueue.Any())
        {
            SpawnCard();
        }

        var card = MonsterQueue.Dequeue();
        ShiftOtherMonsters();
        return card;
    }

    public override void _Ready()
    {
        Instance = this;
        ScreenSize = GetTree().Root.Size;

        GameDataService = GetNode<GameDataService>(GameDataService.Path);

        rand = new RandomNumberGenerator();
        rand.Randomize();

        timeBeforeNextSpawn = SpawnRateInSecs;
    }

    public override void _Process(double delta)
    {
        timeBeforeNextSpawn -= delta;
        if (timeBeforeNextSpawn <= 0)
        {
            timeBeforeNextSpawn = SpawnRateInSecs;

            if (MonsterQueue.Count < MaxInQueue)
            {
                SpawnCard();
            }
        }
    }

    private void SpawnCard()
    {
        var newMonsterCard = MonsterCardUIPrefab.Instantiate<MonsterCardUI>();
        var monsterSpecs = new SummoningSpecs(rand.RandiRange(0, 4), rand.RandiRange(0, 4), rand.RandiRange(0, 4));

        var emotion = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Emotion, monsterSpecs.Emotion.Index));
        var element = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Element, monsterSpecs.Element.Index));
        var species = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Species, monsterSpecs.Species.Index));

        newMonsterCard.IsDraggable = false;
        newMonsterCard.Init(monsterSpecs, emotion, element, species);

        var targetPos = new Vector2(0, 0);
        var spawnPos = new Vector2(-newMonsterCard.Position.X, 50);
        newMonsterCard.Position = spawnPos;
        newMonsterCard.RotationDegrees = 15;
        newMonsterCard.TriggerShiftAnim(targetPos);

        var container = CardContainer ?? this;
        container.AddChild(newMonsterCard);
        container.MoveChild(newMonsterCard, 0);

        MonsterQueue.Enqueue(newMonsterCard);
        ShiftOtherMonsters();
    }

    private void ShiftOtherMonsters()
    {
        float i = MonsterQueue.Count;
        foreach (var monsterCard in MonsterQueue)
        {
            i--;
            var offset = i * 5f;
            var targetPos = new Vector2(offset, offset * 2);
            monsterCard.TriggerShiftAnim(targetPos);
        }
    }
}
