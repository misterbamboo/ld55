using Godot;
using System.Collections.Generic;

public partial class BossQueueManager : Node
{
    private const double SpawnRateInSecs = 2;

    private double timeBeforeNextSpawn;

    private RandomNumberGenerator rand;

    [Export] private PackedScene MonsterCardUIPrefab { get; set; }

    private Queue<MonsterCardUI> MonsterQueue { get; set; } = new Queue<MonsterCardUI>();

    private Vector2I ScreenSize { get; set; }

    private GameDataService GameDataService { get; set; }

    public override void _Ready()
    {
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
            SpawnMonster();
        }
    }

    private void SpawnMonster()
    {
        var newMonsterCard = MonsterCardUIPrefab.Instantiate<MonsterCardUI>();
        var monsterSpecs = new SummoningSpecs(rand.RandiRange(0, 4), rand.RandiRange(0, 4), rand.RandiRange(0, 4));

        var emotion = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Emotion, monsterSpecs.Emotion.Index));
        var element = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Element, monsterSpecs.Element.Index));
        var species = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Species, monsterSpecs.Species.Index));

        newMonsterCard.IsDraggable = false;
        newMonsterCard.Init(monsterSpecs, emotion, element, species);

        var targetPos = new Vector2(ScreenSize.X - (newMonsterCard.Size.X), 0);
        var spawnPos = new Vector2(ScreenSize.X, 50);
        newMonsterCard.Position = spawnPos;
        newMonsterCard.RotationDegrees = 15;
        newMonsterCard.TriggerShiftAnim(targetPos);

        AddChild(newMonsterCard);
        MoveChild(newMonsterCard, 0);

        MonsterQueue.Enqueue(newMonsterCard);
        ShiftOtherMonsters();
    }

    private void ShiftOtherMonsters()
    {
        float i = MonsterQueue.Count;
        foreach (var monsterCard in MonsterQueue)
        {
            i--;
            var offset = i * 15f;
            var targetPos = new Vector2(ScreenSize.X - monsterCard.Size.X - offset, offset);
            monsterCard.TriggerShiftAnim(targetPos);
        }
    }
}
