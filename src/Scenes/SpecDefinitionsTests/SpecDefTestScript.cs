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
        UpdateDisplayText();
    }

    public void ElementChanged(float value)
    {
        PlayerIndex = (int)value;
        var id = $"{SpecTypes.Element}_{PlayerIndex}";
        var spec = GameDataService.GetSpecDefinition(id);
        PlayerElementSpec = spec;
        UpdateDisplayText();
    }

    public void SpeciesChanged(float value)
    {
        PlayerIndex = (int)value;
        var id = $"{SpecTypes.Species}_{PlayerIndex}";
        var spec = GameDataService.GetSpecDefinition(id);
        PlayerSpeciesSpec = spec;
        UpdateDisplayText();
    }

    private void UpdateDisplayText()
    {
        RichTextLabel.Text =
            $"Name: {PlayerEmotionSpec.SpecNaming} {PlayerElementSpec.SpecNaming} {PlayerSpeciesSpec.SpecNaming}" +
            System.Environment.NewLine +
            $"Monster: {PlayerEmotionSpec.MonsterNaming} {PlayerElementSpec.MonsterNaming} {PlayerSpeciesSpec.MonsterNaming}";
    }

    public void EnemyEmotionChanged(float value)
    {
        CombatType = SpecTypes.Emotion;
        BossIndex = (int)value;
        var spec = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(CombatType, BossIndex));
        BossEmotionSpec = spec;
        UpdateDisplayCombatText();
    }
    public void EnemyElementChanged(float value)
    {
        CombatType = SpecTypes.Element;
        BossIndex = (int)value;
        var spec = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(CombatType, BossIndex));
        BossElementSpec = spec;
        UpdateDisplayCombatText();
    }

    public void EnemySpeciesChanged(float value)
    {
        CombatType = SpecTypes.Species;
        BossIndex = (int)value;
        var spec = GameDataService.GetSpecDefinition(SpecDefinition.CreateId(CombatType, BossIndex));
        BossSpeciesSpec = spec;
        UpdateDisplayCombatText();
    }

    private void UpdateDisplayCombatText()
    {
        var hint = GameDataService.GetHintDefinition($"{CombatType}_{PlayerIndex}_{BossIndex}");
        CombatRichTextLabel.Text = hint.Text;
    }

    public void PlayerChanged()
    {
        PlayerMonsterCardUI.Init(new SummoningSpecs(PlayerEmotionSpec.Index, PlayerElementSpec.Index, PlayerSpeciesSpec.Index));
        PlayerMonsterCardUI.RedrawMonster();
    }

    public void BossChanged()
    {
        BossMonsterCardUI.Init(new SummoningSpecs(BossEmotionSpec.Index, BossElementSpec.Index, BossSpeciesSpec.Index));
        BossMonsterCardUI.RedrawMonster();
    }
}
