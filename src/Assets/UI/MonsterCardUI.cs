using Godot;
using System;

public partial class MonsterCardUI : Control
{
    [Export] private RichTextLabel NameLabel { get; set; }
    [Export] private PanelContainer SpeciesImage { get; set; }
    [Export] private PanelContainer EmotionImage { get; set; }
    private MonsterImageLoader MonsterImageLoader { get; set; }
    private GameDataService GameDataService { get; set; }
    public SummoningSpecs MonsterSpecs { get; set; }

    public void Init(SummoningSpecs monsterSpecs)
    {
        MonsterSpecs = monsterSpecs;
    }

    public override void _Ready()
    {
        RedrawMonster();
    }

    private void ChangeName(string name)
    {
        NameLabel.Text = name;
    }

    private void ChangeImage(MonsterImageResult imageResult)
    {
        //SpeciesImage
        //EmotionImage
    }

    public void RedrawMonster()
    {
        GameDataService = GetNode<GameDataService>(GameDataService.Path);
        var emotion = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Emotion, MonsterSpecs.Emotion.Index));
        var element = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Element, MonsterSpecs.Element.Index));
        var species = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Species, MonsterSpecs.Species.Index));
        ChangeName($"{emotion.MonsterNaming} {element.MonsterNaming} {species.MonsterNaming}");

        MonsterImageLoader = GetNode<MonsterImageLoader>(MonsterImageLoader.Path);
        var imageResult = MonsterImageLoader.GetMonsterImage(MonsterSpecs);
        ChangeImage(imageResult);
    }
}
