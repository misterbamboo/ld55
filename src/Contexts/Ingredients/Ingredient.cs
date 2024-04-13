public class Ingredient
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Icon { get; set; }
    public string ProcessedIcon { get; set; }
    public RotatingValue Emotion { get; set; }
    public RotatingValue Species { get; set; }
    public RotatingValue Elements { get; set; }

    public Ingredient(string id, string displayName, string icon, string processedIcon, RotatingValue emotion, RotatingValue species, RotatingValue elements)
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
        Emotion = new RotatingValue(emotion);
        Species = new RotatingValue(species);
        Elements = new RotatingValue(elements);
    }
}