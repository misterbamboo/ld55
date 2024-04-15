using Godot;

public partial class SpecDefTestScript : Node
{
    [Export] public RichTextLabel RichTextLabel { get; set; }

    [Export] public RichTextLabel CombatRichTextLabel { get; set; }

    [Export] public MonsterCardUI PlayerMonsterCardUI { get; set; }

    [Export] public MonsterCardUI BossMonsterCardUI { get; set; }

    private GameDataService GameDataService { get; set; }
    private SpecDefinition PlayerEmotionSpec { get; set; } = SpecDefinition.Empty();
    private SpecDefinition PlayerSpeciesSpec { get; set; } = SpecDefinition.Empty();
    private SpecDefinition PlayerElementSpec { get; set; } = SpecDefinition.Empty();
    private SpecDefinition BossEmotionSpec { get; set; } = SpecDefinition.Empty();
    private SpecDefinition BossSpeciesSpec { get; set; } = SpecDefinition.Empty();
    private SpecDefinition BossElementSpec { get; set; } = SpecDefinition.Empty();
    public SpecTypes CombatType { get; private set; }
    public int BossIndex { get; private set; }
    public int PlayerIndex { get; private set; }

    public override void _Ready()
    {
        GameDataService = GetNode<GameDataService>(GameDataService.Path);
    }

    public override void _Process(double delta)
    {
    }

    public void EmotionChanged(float value)
    {
        PlayerIndex = (int)value;
        var id = $"{SpecTypes.Emotion}_{PlayerIndex}";
        var spec = GameDataService.GetSpecDefinition(id);
        PlayerEmotionSpec = spec;
        PlayerChanged();

    }

    public void ElementChanged(float value)
    {
        PlayerIndex = (int)value;
        var id = $"{SpecTypes.Element}_{PlayerIndex}";
        var spec = GameDataService.GetSpecDefinition(id);
        PlayerElementSpec = spec;
        PlayerChanged();
    }

    public void SpeciesChanged(float value)
    {
        PlayerIndex = (int)value;
        var id = $"{SpecTypes.Species}_{PlayerIndex}";
        var spec = GameDataService.GetSpecDefinition(id);
        PlayerSpeciesSpec = spec;
        PlayerChanged();
    }

    public void EnemyEmotionChanged(float value)
    {
        CombatType = SpecTypes.Emotion;
        BossIndex = (int)value;
        var spec = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(CombatType, BossIndex));
        BossEmotionSpec = spec;
        BossChanged();

    }
    public void EnemyElementChanged(float value)
    {
        CombatType = SpecTypes.Element;
        BossIndex = (int)value;
        var spec = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(CombatType, BossIndex));
        BossElementSpec = spec;
        BossChanged();
    }

    public void EnemySpeciesChanged(float value)
    {
        CombatType = SpecTypes.Species;
        BossIndex = (int)value;
        var spec = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(CombatType, BossIndex));
        BossSpeciesSpec = spec;
        BossChanged();
    }

    public void PlayerChanged()
    {
        var playerSpecs = new SummoningSpecs(PlayerEmotionSpec.Index, PlayerElementSpec.Index, PlayerSpeciesSpec.Index);
        var emotion = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Emotion, playerSpecs.Emotion.Index));
        var element = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Element, playerSpecs.Element.Index));
        var species = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Species, playerSpecs.Species.Index));

        PlayerMonsterCardUI.Init(playerSpecs, emotion, element, species);
        PlayerMonsterCardUI.RedrawMonster();
        UpdateDisplayText();
    }

    private void UpdateDisplayText()
    {
        RichTextLabel.Text =
            $"Name: {PlayerEmotionSpec.SpecNaming} {PlayerElementSpec.SpecNaming} {PlayerSpeciesSpec.SpecNaming}" +
            System.Environment.NewLine +
            $"Monster: {PlayerEmotionSpec.MonsterNaming} {PlayerElementSpec.MonsterNaming} {PlayerSpeciesSpec.MonsterNaming}";
    }

    public void BossChanged()
    {
        var bossSpecs = new SummoningSpecs(BossEmotionSpec.Index, BossElementSpec.Index, BossSpeciesSpec.Index);
        var emotion = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Emotion, bossSpecs.Emotion.Index));
        var element = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Element, bossSpecs.Element.Index));
        var species = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Species, bossSpecs.Species.Index));

        BossMonsterCardUI.Init(bossSpecs, emotion, element, species);
        BossMonsterCardUI.RedrawMonster();

        var bossFight = new BossFight(GameDataService);
        var playerSpecs = new SummoningSpecs(PlayerEmotionSpec.Index, PlayerElementSpec.Index, PlayerSpeciesSpec.Index);
        bossFight.Combat(playerSpecs, bossSpecs);

        UpdateDisplayCombatText(bossFight.Result, playerSpecs, bossSpecs);
    }

    private void UpdateDisplayCombatText(BossFight.BossFightResult result, SummoningSpecs playerSpecs, SummoningSpecs bossSpecs)
    {
        string id = HintDef.CreateId(CombatType, PlayerIndex, BossIndex);
        var hint = GameDataService.GetHintDefinition(id);
        string line1 = $"{result}. {hint.Text}";
        string line2 = $"player: {playerSpecs}";
        string line3 = $"boss: {bossSpecs}";
        var ln = System.Environment.NewLine;
        CombatRichTextLabel.Text = $"{line1}{ln}{line2}{ln}{line3}";
    }
}
