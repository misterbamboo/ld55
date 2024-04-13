using Godot;

public class Ingredient
{
    public const string None = "None";

    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Icon { get; set; }
    public string ProcessedIcon { get; set; }
    public SummoningValue Emotion { get; set; }
    public SummoningValue Species { get; set; }
    public SummoningValue Elements { get; set; }
    public float Processing { get; set; }

    public Ingredient(string id, string displayName, string icon, string processedIcon, SummoningValue emotion, SummoningValue species, SummoningValue elements)
    {
        Id = id;
        DisplayName = displayName;
        Icon = icon;
        ProcessedIcon = processedIcon;
        Emotion = emotion;
        Species = species;
        Elements = elements;
    }

    public Ingredient(string id, string displayName, string icon, string processedIcon, float emotion, float species, float elements)
    {
        Id = id;
        DisplayName = displayName;
        Icon = icon;
        ProcessedIcon = processedIcon;
        Emotion = new SummoningValue(emotion);
        Species = new SummoningValue(species);
        Elements = new SummoningValue(elements);
    }

    public static Ingredient Void()
    {
        return new Ingredient(Ingredient.None, None, None, None, 0,0,0);
    }

    public bool IsVoid => Id == None;

    
}