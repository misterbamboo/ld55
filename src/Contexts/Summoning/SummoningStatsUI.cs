using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class SummoningStatsUI : Node
{
	private DeskManager deskManager;
	private PropertyBarUI EmotionBar;
	private PropertyBarUI SpeciesBar;
	private PropertyBarUI ElementBar;
	private RandomNumberGenerator rng = new RandomNumberGenerator();

	private int emotionDeadZone;
	private int speciesDeadZone;
	private int elementDeadZone;

	private RotatingValue baseEmotion;
	private RotatingValue baseSpecies;
	private RotatingValue baseElement;

	private RotatingValue currentEmotion;
	private RotatingValue currentSpecies;
	private RotatingValue currentElement;

    public override void _Ready()
	{
		deskManager = GetNode<DeskManager>(DeskManager.Path);
		EmotionBar = GetNode<PropertyBarUI>("VBoxContainer/EmotionBar");
		SpeciesBar = GetNode<PropertyBarUI>("VBoxContainer/SpeciesBar");
		ElementBar = GetNode<PropertyBarUI>("VBoxContainer/ElementBar");

		deskManager.OnArcaneFocusAdjusted += UpdateArcaneFocus;
		deskManager.OnIngredientsOnSummoningBoardUpdated += UpdateIngredients;
		deskManager.OnArcaneFocusFilled += UpdateArcaneFocus;
        deskManager.OnMonsterSummoned += OnMonsterSummoned;
		deskManager.OnGameStart += OnGameStart;
    }

    private void OnMonsterSummoned(SummoningSpecs _)
    {
		StartNewSummon();
    }

	private void OnGameStart()
	{
        StartNewSummon();
    }

	private void StartNewSummon()
	{
		emotionDeadZone = rng.RandiRange(0, 5);
		speciesDeadZone = rng.RandiRange(0, 5);
        elementDeadZone = rng.RandiRange(0, 5);

        currentEmotion = baseEmotion = new RotatingValue(emotionDeadZone + 0.5, 6);
		currentSpecies = baseSpecies = new RotatingValue(speciesDeadZone + 0.5, 6);
		currentElement = baseElement = new RotatingValue(elementDeadZone + 0.5, 6);

		InitBars(emotionDeadZone, speciesDeadZone, elementDeadZone);
		UpdateIngredients(Enumerable.Empty<Ingredient>());
    }

    public void InitBars(int emotionDeadzone, int speciesDeadzone, int elementDeadzone)
	{
		EmotionBar.Init(SpecTypes.Emotion, emotionDeadzone);
	    SpeciesBar.Init(SpecTypes.Species, speciesDeadzone);
	    ElementBar.Init(SpecTypes.Element, elementDeadzone);
	}

	public void UpdateIngredients(IEnumerable<Ingredient> ingredients)
	{
		var totalEmotion = ingredients.Sum(i => i.EmotionValue);
		var totalSpecies = ingredients.Sum(i => i.SpeciesValue);
		var totalElement = ingredients.Sum(i => i.ElementValue);

		currentEmotion = new RotatingValue(baseEmotion.Value + totalEmotion, 6);
		currentSpecies = new RotatingValue(baseSpecies.Value + totalSpecies, 6);
		currentElement = new RotatingValue(baseElement.Value + totalElement, 6);

        EmotionBar.SetValue(currentEmotion.Value / 6);
		SpeciesBar.SetValue(currentSpecies.Value / 6);
		ElementBar.SetValue(currentElement.Value / 6);

		if(!IsInDeadZone(currentEmotion.Value, emotionDeadZone) && !IsInDeadZone(currentSpecies.Value, speciesDeadZone) && !IsInDeadZone(currentElement.Value, elementDeadZone))
		{
			var ddemotion = DeDeadzoneIfy(currentEmotion, emotionDeadZone);
			var ddspecies = DeDeadzoneIfy(currentSpecies, speciesDeadZone);
			var ddelement = DeDeadzoneIfy(currentElement, elementDeadZone);

            var monsterSpecs = new SummoningSpecs(ddemotion, ddelement, ddspecies);

            deskManager.ReadyMonsterForSummon(monsterSpecs);
		}
    }

	public void UpdateArcaneFocus(Ingredient ingredient)
	{
        EmotionBar.SetPreview(new RotatingValue(currentEmotion.Value + ingredient.EmotionValue, 6).Value / 6);
        SpeciesBar.SetPreview(new RotatingValue(currentSpecies.Value + ingredient.SpeciesValue, 6).Value / 6);
        ElementBar.SetPreview(new RotatingValue(currentElement.Value + ingredient.ElementValue, 6).Value / 6);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
		deskManager.OnArcaneFocusAdjusted -= UpdateArcaneFocus;
		deskManager.OnIngredientsOnSummoningBoardUpdated -= UpdateIngredients;
		deskManager.OnArcaneFocusFilled -= UpdateArcaneFocus;
    }

	public RotatingValue DeDeadzoneIfy(RotatingValue value, int deadzone)
	{
		if (value.Value >= deadzone + 1)
		{
            GD.Print($"DeDeadzoning: {value.Value}, to {new RotatingValue(value.Value - 1, 5)} with deadzone: {deadzone}");
            return new RotatingValue(value.Value -1, 5);
		}

        GD.Print($"DeDeadzoning: {value.Value}, to {new RotatingValue(value.Value, 5)} with deadzone: {deadzone}");
        return new RotatingValue(value.Value, 5);
    }

	public bool IsInDeadZone(double value, int deadzone)
	{
        return value >= deadzone && value < deadzone + 1;
    }
}
