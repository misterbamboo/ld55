using Godot;
using System;

public partial class SpecDefTestScript : Node
{
    [Export] public RichTextLabel RichTextLabel { get; set; }

    private GameDataService GameDataService { get; set; }
    private SpecDefinition EmotionSpec { get; set; } = SpecDefinition.Empty();
    private SpecDefinition SpeciesSpec { get; set; } = SpecDefinition.Empty();
    private SpecDefinition ElementSpec { get; set; } = SpecDefinition.Empty();

    public override void _Ready()
    {
        GameDataService = GetNode<GameDataService>(GameDataService.Path);
    }

    public override void _Process(double delta)
    {
    }

    public void EmotionChanged(float value)
    {
        var index = (int)value;
        var id = $"{SpecTypes.Emotion}_{index}";
        var spec = GameDataService.GetSpecDefinition(id);
        EmotionSpec = spec;
        UpdateDisplayText();
    }

    public void ElementChanged(float value)
    {
        var index = (int)value;
        var id = $"{SpecTypes.Element}_{index}";
        var spec = GameDataService.GetSpecDefinition(id);
        ElementSpec = spec;
        UpdateDisplayText();
    }

    public void SpeciesChanged(float value)
    {
        var index = (int)value;
        var id = $"{SpecTypes.Species}_{index}";
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
}
