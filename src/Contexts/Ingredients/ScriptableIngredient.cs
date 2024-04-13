using Godot;

public partial class ScriptableIngredient : Resource
{
    [Export] public string Id { get; set; }
    [Export] public string DisplayName { get; set; }
    [Export] public string Icon { get; set; }
    [Export] public string ProcessedIcon { get; set; }
    [Export] public float Emotion { get; set; }
    [Export] public float Species { get; set; }
    [Export] public float Elements { get; set; }

    public Ingredient ToEntity()
    {
        return new Ingredient(
            Id,
            DisplayName,
            Icon,
            ProcessedIcon,
            Emotion,
            Species,
            Elements);
    }
}

