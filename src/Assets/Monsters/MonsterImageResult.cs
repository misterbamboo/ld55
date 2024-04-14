using Godot;

public class MonsterImageResult
{
    public MonsterImageResult(int speciesIndex, int elementIndex, int emotionIndex, ImageTexture speciesElementImage, ImageTexture speciesEmotionImage)
    {
        SpeciesIndex = speciesIndex;
        ElementIndex = elementIndex;
        EmotionIndex = emotionIndex;
        SpeciesImage = speciesElementImage;
        EmotionImage = speciesEmotionImage;
    }

    public int SpeciesIndex { get; }
    public int ElementIndex { get; }
    public int EmotionIndex { get; }
    public ImageTexture SpeciesImage { get; }
    public ImageTexture EmotionImage { get; }
}