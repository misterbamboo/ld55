using Godot;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

public partial class MonsterImageLoader : Node
{
    public const string Path = "/root/MonsterImageLoader";

    private const string NoImagePath = "res://Assets/Monsters/default.png";

    private GameDataService GameDataService { get; set; }

    private List<MonsterImageLoaderCache> SpecDefCache { get; set; } = new List<MonsterImageLoaderCache>();

    private List<MonsterImageResult> MonsterImageResults = new List<MonsterImageResult>();

    public override void _Ready()
    {
        GameDataService = GetNode<GameDataService>(GameDataService.Path);
        LoadCacheSpecs();
    }

    private void LoadCacheSpecs()
    {
        for (int i = 0; i < 3; i++)
        {
            var type = (SpecTypes)i;
            for (int j = 0; j < 5; j++)
            {
                string id = SpecDefinition.CreateId(type, j);
                var spec = GameDataService.GetSpecDefinition(id);
                SpecDefCache.Add(new MonsterImageLoaderCache(type, j, spec));
            }
        }
    }

    public MonsterImageResult GetMonsterImage(SummoningSpecs summoningSpecs)
    {
        var monsterImageResult = GetMonsterImageResultInCache(summoningSpecs.Species.Index, summoningSpecs.Element.Index, summoningSpecs.Emotion.Index);
        if (monsterImageResult == default)
        {
            monsterImageResult = LoadMonsterImageResultInCache(summoningSpecs.Species.Index, summoningSpecs.Element.Index, summoningSpecs.Emotion.Index);
        }

        return monsterImageResult;
    }

    private MonsterImageResult GetMonsterImageResultInCache(int speciesIndex, int elementIndex, int emotionIndex)
    {
        return MonsterImageResults.FirstOrDefault(r => r.SpeciesIndex == speciesIndex && r.ElementIndex == elementIndex && r.EmotionIndex == emotionIndex);
    }

    private MonsterImageResult LoadMonsterImageResultInCache(int speciesIndex, int elementIndex, int emotionIndex)
    {
        var speciesElementImagePath = GetSpeciesElementImagePath(speciesIndex, elementIndex);
        var speciesEmotionImagePath = GetSpeciesElementEmotionImagePath(speciesIndex, emotionIndex);
        var speciesIconFilePath = GetIconImagePath(SpecTypes.Species, speciesIndex);
        var elementIconFilePath = GetIconImagePath(SpecTypes.Element, elementIndex);
        var emotionIconFilePath = GetIconImagePath(SpecTypes.Emotion, emotionIndex);

        var speciesElementImage = LoadImageFromFile(speciesElementImagePath);
        var speciesEmotionImage = LoadImageFromFile(speciesEmotionImagePath);
        var speciesIconImage = LoadImageFromFile(speciesIconFilePath);
        var elementIconImage = LoadImageFromFile(elementIconFilePath);
        var emotionIconImage = LoadImageFromFile(emotionIconFilePath);

        var monsterImageResult = new MonsterImageResult(speciesIndex, elementIndex, emotionIndex, speciesElementImage, speciesEmotionImage, speciesIconImage, elementIconImage, emotionIconImage);
        MonsterImageResults.Add(monsterImageResult);
        return monsterImageResult;
    }

    private string GetIconImagePath(SpecTypes specType, int index)
    {
        var spec = SpecDefCache.Where(c => c.Type == specType && c.Index == index).FirstOrDefault();
        var specName = spec.Spec.SpecNaming;
        var iconFilePath = $"res://Assets/Monsters/Icons/{specName}.png";
        return iconFilePath;
    }

    private string GetSpeciesElementImagePath(int species, int element)
    {
        var speciesSpec = SpecDefCache.Where(c => c.Type == SpecTypes.Species && c.Index == species).FirstOrDefault();
        var elementSpec = SpecDefCache.Where(c => c.Type == SpecTypes.Element && c.Index == element).FirstOrDefault();

        var speciesName = speciesSpec.Spec.SpecNaming;
        var elementName = elementSpec.Spec.SpecNaming;

        var folderPath = $"res://Assets/Monsters/{speciesName}";
        var speciesElementFile = $"{speciesName}_{elementName}.png";
        var speciesElementPath = $"{folderPath}/{speciesElementFile}";

        return speciesElementPath;
    }

    private string GetSpeciesElementEmotionImagePath(int species, int emotion)
    {
        var speciesSpec = SpecDefCache.Where(c => c.Type == SpecTypes.Species && c.Index == species).FirstOrDefault();
        var emotionSpec = SpecDefCache.Where(c => c.Type == SpecTypes.Emotion && c.Index == emotion).FirstOrDefault();

        var speciesName = speciesSpec.Spec.SpecNaming;
        var emotionName = emotionSpec.Spec.SpecNaming;

        var folderPath = $"res://Assets/Monsters/{speciesName}/Emotions";
        var speciesEmotionFile = $"{speciesName}_{emotionName}.png";
        var speciesEmotionPath = $"{folderPath}/{speciesEmotionFile}";

        return speciesEmotionPath;
    }

    private static Texture2D LoadImageFromFile(string speciesElementPath)
    {
        Texture2D texture = null;
        if (ResourceLoader.Exists(speciesElementPath))
        {
            var res = GD.Load(speciesElementPath);
            return (Texture2D)res;
        }
        else
        {
            return (Texture2D)GD.Load(NoImagePath);
        }
    }

    private class MonsterImageLoaderCache
    {
        public MonsterImageLoaderCache(SpecTypes type, int index, SpecDefinition spec)
        {
            Type = type;
            Index = index;
            Spec = spec;
        }

        public SpecTypes Type { get; }
        public int Index { get; }
        public SpecDefinition Spec { get; }
    }
}
