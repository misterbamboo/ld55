using Godot;
using System;

public partial class SpecDefTestScript : Node
{
    [Export] public RichTextLabel RichTextLabel { get; set; }

    [Export] public RichTextLabel CombatRichTextLabel { get; set; }

    private GameDataService GameDataService { get; set; }
    private SpecDefinition EmotionSpec { get; set; } = SpecDefinition.Empty();
    private SpecDefinition SpeciesSpec { get; set; } = SpecDefinition.Empty();
    private SpecDefinition ElementSpec { get; set; } = SpecDefinition.Empty();
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
        EmotionSpec = spec;
        UpdateDisplayText();
    }

    public void ElementChanged(float value)
    {
        PlayerIndex = (int)value;
        var id = $"{SpecTypes.Element}_{PlayerIndex}";
        var spec = GameDataService.GetSpecDefinition(id);
        ElementSpec = spec;
        UpdateDisplayText();
    }

    public void SpeciesChanged(float value)
    {
        PlayerIndex = (int)value;
        var id = $"{SpecTypes.Species}_{PlayerIndex}";
        var spec = GameDataService.GetSpecDefinition(id);
        SpeciesSpec = spec;
        UpdateDisplayText();
    }

    private void UpdateDisplayText()
    {
        RichTextLabel.Text =
            $"Name: {EmotionSpec.SpecNaming} {ElementSpec.SpecNaming} {SpeciesSpec.SpecNaming}" +
            System.Environment.NewLine +
            $"Monster: {EmotionSpec.MonsterNaming} {ElementSpec.MonsterNaming} {SpeciesSpec.MonsterNaming}";
    }

    public void EnemyEmotionChanged(float value)
    {
        CombatType = SpecTypes.Emotion;
        BossIndex = (int)value;
        UpdateDisplayCombatText();
    }

    public void EnemyElementChanged(float value)
    {
        CombatType = SpecTypes.Element;
        BossIndex = (int)value;
        UpdateDisplayCombatText();
    }

    public void EnemySpeciesChanged(float value)
    {
        CombatType = SpecTypes.Species;
        BossIndex = (int)value;
        UpdateDisplayCombatText();
    }
    private void UpdateDisplayCombatText()
    {
        var hint = GameDataService.GetHintDefinition($"{CombatType}_{PlayerIndex}_{BossIndex}");
        CombatRichTextLabel.Text = hint.Text;
    }
}
