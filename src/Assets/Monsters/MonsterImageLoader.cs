using Godot;
using System.Collections.Generic;
using System.Linq;

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
        var speciesElementPath = GetSpeciesElementImagePath(speciesIndex, elementIndex);
        var speciesEmotionPath = GetSpeciesElementEmotionImagePath(speciesIndex, emotionIndex);
        var speciesElementImage = LoadImageFromFile(speciesElementPath);
        var speciesEmotionImage = LoadImageFromFile(speciesEmotionPath);

        var monsterImageResult = new MonsterImageResult(speciesIndex, elementIndex, emotionIndex, speciesElementImage, speciesEmotionImage);
        MonsterImageResults.Add(monsterImageResult);
        return monsterImageResult;
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

    private static ImageTexture LoadImageFromFile(string speciesElementPath)
    {
        Image image = null;
        GD.Print($"Try loading monster at: {speciesElementPath}");
        if (ResourceLoader.Exists(speciesElementPath))
        {
            image = Image.LoadFromFile(speciesElementPath);
        }
        else
        {
            image = Image.LoadFromFile(NoImagePath);
        }
        return ImageTexture.CreateFromImage(image);
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
