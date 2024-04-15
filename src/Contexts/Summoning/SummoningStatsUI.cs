using Godot;

public partial class SummoningStatsUI : Node
{
	private DeskManager deskManager;
	private PropertyBarUI EmotionBar;
	private PropertyBarUI SpeciesBar;
	private PropertyBarUI ElementBar;

    public override void _Ready()
	{
		deskManager = GetNode<DeskManager>(DeskManager.Path);
		EmotionBar = GetNode<PropertyBarUI>("VBoxContainer/EmotionBar");
		SpeciesBar = GetNode<PropertyBarUI>("VBoxContainer/SpeciesBar");
		ElementBar = GetNode<PropertyBarUI>("VBoxContainer/ElementBar");

		SetPreviews(new SummoningSpecs(2.5, 2.5, 2.5));
		SetBars(new SummoningSpecs(2.5, 2.5, 2.5));

		deskManager.OnArcaneFocusAdjusted += SetPreviews;
		deskManager.OnMonsterStatsUpdated += SetBars;
		deskManager.OnArcaneFocusFilled += SetPreviews;
    }

	public void SetBars(SummoningSpecs specs)
	{
		EmotionBar.SetValue(specs.Emotion.Value / 5);
        SpeciesBar.SetValue(specs.Species.Value / 5);
        ElementBar.SetValue(specs.Element.Value / 5);
    }

	public void SetPreviews(SummoningSpecs specs)
	{
        EmotionBar.SetPreview((deskManager.CurrentMonsterStats.Emotion + specs.Emotion).Value / 5);
        SpeciesBar.SetPreview((deskManager.CurrentMonsterStats.Species + specs.Species).Value / 5);
        ElementBar.SetPreview((deskManager.CurrentMonsterStats.Element + specs.Element).Value / 5);
    }

	public void SetPreviews(Ingredient ingredient)
	{
		var emotionValue = (deskManager.CurrentMonsterStats.Emotion + ingredient.EmotionValue).Value;
		var speciesValue = (deskManager.CurrentMonsterStats.Species + ingredient.SpeciesValue).Value;
		var elementValue = (deskManager.CurrentMonsterStats.Element + ingredient.ElementValue).Value;

        EmotionBar.SetPreview(emotionValue / 5);
        SpeciesBar.SetPreview(speciesValue / 5);
        ElementBar.SetPreview(elementValue / 5);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
		deskManager.OnArcaneFocusAdjusted -= SetPreviews;
		deskManager.OnMonsterStatsUpdated -= SetBars;
		deskManager.OnArcaneFocusFilled -= SetPreviews;
    }
}
