using Godot;

public partial class MonsterCardUI : Control
{
    [Export] private RichTextLabel NameLabel { get; set; }

    [Export] private TextureRect SpeciesImage { get; set; }
    [Export] private TextureRect EmotionImage { get; set; }

    [Export] private TextureRect EmotionIconImage { get; set; }
    [Export] private TextureRect ElementIconImage { get; set; }
    [Export] private TextureRect SpeciesIconImage { get; set; }

    private MonsterImageLoader MonsterImageLoader { get; set; }
    public SummoningSpecs MonsterSpecs { get; set; } = new SummoningSpecs();

    private SpecDefinition Emotion { get; set; } = SpecDefinition.Empty();
    private SpecDefinition Element { get; set; } = SpecDefinition.Empty();
    private SpecDefinition Species { get; set; } = SpecDefinition.Empty();

    public void Init(SummoningSpecs monsterSpecs, SpecDefinition emotion, SpecDefinition element, SpecDefinition species)
    {
        MonsterSpecs = monsterSpecs;
        Emotion = emotion;
        Element = element;
        Species = species;
    }

    public override void _Ready()
    {
        MonsterImageLoader = GetNode<MonsterImageLoader>(MonsterImageLoader.Path);

        RedrawMonster();
    }

    private void ChangeName(string name)
    {
        NameLabel.Text = name;
    }

    private void ChangeImageAndIcons(MonsterImageResult imageResult)
    {
        SpeciesImage.Texture = imageResult.SpeciesImage;
        EmotionImage.Texture = imageResult.EmotionImage;
        EmotionIconImage.Texture = imageResult.EmotionIconImage;
        ElementIconImage.Texture = imageResult.ElementIconImage;
        SpeciesIconImage.Texture = imageResult.SpeciesIconImage;
    }

    public void RedrawMonster()
    {
        ChangeName($"{Emotion.MonsterNaming} {Element.MonsterNaming} {Species.MonsterNaming}");

        var imageResult = MonsterImageLoader.GetMonsterImage(MonsterSpecs);
        ChangeImageAndIcons(imageResult);
    }
}
