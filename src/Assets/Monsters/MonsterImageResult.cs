using Godot;

public class MonsterImageResult
{
    public MonsterImageResult(int speciesIndex, int elementIndex, int emotionIndex, Image speciesElementImage, Image speciesEmotionImage)
    {
        SpeciesIndex = speciesIndex;
        ElementIndex = elementIndex;
        EmotionIndex = emotionIndex;
        SpeciesElementImage = speciesElementImage;
        SpeciesEmotionImage = speciesEmotionImage;
    }

    public int SpeciesIndex { get; }
    public int ElementIndex { get; }
    public int EmotionIndex { get; }
    public Image SpeciesElementImage { get; }
    public Image SpeciesEmotionImage { get; }
}