using Godot;

public partial class MonsterCardUI : Control
{
    [Export] private RichTextLabel NameLabel { get; set; }

    private GameDataService GameDataService { get; set; }

    public SummoningSpecs MonsterSpecs { get; set; }

    public override void _Ready()
    {
        GameDataService = GetNode<GameDataService>(GameDataService.Path);
        var emotionId = $"{SpecTypes.Emotion}_{MonsterSpecs.Emotion.Index}";
        var elementId = $"{SpecTypes.Element}_{MonsterSpecs.Element.Index}";
        var speciesId = $"{SpecTypes.Species}_{MonsterSpecs.Species.Index}";

        var emotion = GameDataService.GetSpecDefinition(emotionId);
        var element = GameDataService.GetSpecDefinition(elementId);
        var species = GameDataService.GetSpecDefinition(speciesId);

        ChangeName($"{emotion.MonsterNaming} {element.MonsterNaming} {species.MonsterNaming}");
    }

    public override void _Process(double delta)
    {
    }

    private void ChangeName(string name)
    {
        NameLabel.Text = name;
    }

    public void Init(SummoningSpecs monsterSpecs)
    {
        MonsterSpecs = monsterSpecs;
    }
}
