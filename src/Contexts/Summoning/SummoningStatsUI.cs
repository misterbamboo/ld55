using Godot;

public partial class SummoningStatsUI : Node
{
	private PropertyBarUI EmotionBar;
	private PropertyBarUI SpeciesBar;
	private PropertyBarUI ElementBar;
	// Called when the node enters the scene tree for the first time.

	public SummoningSpecs monsterSpecs;
	private SummoningSpecs previewSpecs;

    public override void _Ready()
	{
		EmotionBar = GetNode<PropertyBarUI>("EmotionBar");
		SpeciesBar = GetNode<PropertyBarUI>("SpeciesBar");
		ElementBar = GetNode<PropertyBarUI>("ElementBar");

		SetPreviews(new SummoningSpecs(2.5, 2.5, 2.5));
		SetBars(new SummoningSpecs(2.5, 2.5, 2.5));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void SetBars(SummoningSpecs specs)
	{
		previewSpecs = specs;

		EmotionBar.SetValue(specs.Emotion.Value / 5);
        SpeciesBar.SetValue(specs.Species.Value / 5);
        ElementBar.SetValue(specs.Element.Value / 5);
    }

	public void SetPreviews(SummoningSpecs specs)
	{
		monsterSpecs = specs;

        EmotionBar.SetPreview(specs.Emotion.Value / 5);
        SpeciesBar.SetPreview(specs.Species.Value / 5);
        ElementBar.SetPreview(specs.Element.Value / 5);
    }
}
