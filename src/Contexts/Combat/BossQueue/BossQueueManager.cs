using Godot;
using System;

public partial class BossQueueManager : Node
{
    private double time;
    int mult = 0;


    private RandomNumberGenerator rand;

    [Export] private PackedScene MonsterCardUIPrefab { get; set; }
    private GameDataService GameDataService { get; set; }

    public override void _Ready()
    {
        GameDataService = GetNode<GameDataService>(GameDataService.Path);

        rand = new RandomNumberGenerator();
        rand.Randomize();
    }

    public override void _Process(double delta)
    {
        time -= delta;
        if (time <= 0)
        {
            time = 0.5;
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

        newMonsterCard.Init(monsterSpecs, emotion, element, species);

        mult++;
        newMonsterCard.Position = new Vector2(10 * mult, 10 * mult);
        AddChild(newMonsterCard);
    }
}
