using Godot;

public class MonsterImageResult
{
    public MonsterImageResult(
        int speciesIndex,
        int elementIndex,
        int emotionIndex,
        Texture2D speciesElementImage,
        Texture2D speciesEmotionImage,
        Texture2D speciesIconImage,
        Texture2D elementIconImage,
        Texture2D emotionIconImage)
    {
        SpeciesIndex = speciesIndex;
        ElementIndex = elementIndex;
        EmotionIndex = emotionIndex;
        SpeciesImage = speciesElementImage;
        EmotionImage = speciesEmotionImage;
        SpeciesIconImage = speciesIconImage;
        ElementIconImage = elementIconImage;
        EmotionIconImage = emotionIconImage;
    }

    public int SpeciesIndex { get; }
    public int ElementIndex { get; }
    public int EmotionIndex { get; }
    public Texture2D SpeciesImage { get; }
    public Texture2D EmotionImage { get; }
    public Texture2D SpeciesIconImage { get; }
    public Texture2D ElementIconImage { get; }
    public Texture2D EmotionIconImage { get; }
}