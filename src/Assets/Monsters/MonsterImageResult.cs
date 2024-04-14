using Godot;

public class MonsterImageResult
{
    public MonsterImageResult(
        int speciesIndex,
        int elementIndex,
        int emotionIndex,
        ImageTexture speciesElementImage,
        ImageTexture speciesEmotionImage,
        ImageTexture speciesIconImage,
        ImageTexture elementIconImage,
        ImageTexture emotionIconImage)
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
    public ImageTexture SpeciesImage { get; }
    public ImageTexture EmotionImage { get; }
    public ImageTexture SpeciesIconImage { get; }
    public ImageTexture ElementIconImage { get; }
    public ImageTexture EmotionIconImage { get; }
}