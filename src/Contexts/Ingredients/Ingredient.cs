using Godot;

public class Ingredient
{
    public const string None = "None";

    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Icon { get; set; }
    public string ProcessedIcon { get; set; }
    public SummoningSpecs SummoningSpecs { get; set; }
   
    public float Processing { get; set; }

    public Ingredient(string id, string displayName, string icon, string processedIcon, RotatingValue emotion, RotatingValue species, RotatingValue element)
    {
        Id = id;
        DisplayName = displayName;
        Icon = icon;
        ProcessedIcon = processedIcon;
        SummoningSpecs = new SummoningSpecs(emotion, element, species);
    }

    public Ingredient(string id, string displayName, string icon, string processedIcon, double emotion, double species, double element)
    {
        Id = id;
        DisplayName = displayName;
        Icon = icon;
        ProcessedIcon = processedIcon;
        SummoningSpecs = new SummoningSpecs(emotion, element, species);
    }

    public static Ingredient Void()
    {
        return new Ingredient(Ingredient.None, None, None, None, 0,0,0);
    }

    public bool IsVoid => Id == None;

    
}