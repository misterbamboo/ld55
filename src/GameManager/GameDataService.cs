using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

public partial class GameDataService : Node, IHintProvider
{
    public static string Path = "/root/GameDataService";

    private Dictionary<string, Ingredient> ingredientsById;
    private Dictionary<string, SpecDefinition> specDefinitionsById;
    private Dictionary<string, HintDef> HintDefById;

    public IEnumerable<Ingredient> Ingredients => ingredientsById.Select(kvp => kvp.Value);

    public override void _Ready()
    {
        var ingredients = LoadAssetsRecursive<ScriptableIngredient>("Ingredients");
        ingredientsById = ingredients.Select(l => l.ToEntity()).ToDictionary(l => l.Id, l => l);

        var specDefinitions = LoadAssetsRecursive<ScriptableSpecDefinition>("SpecDefinitions");
        specDefinitionsById = specDefinitions.Select(l => l.ToEntity()).ToDictionary(l => l.Id, l => l);

        var hintDefinitions = LoadAssetsRecursive<ScriptableHintDef>("Hints");
        HintDefById = hintDefinitions.Select(l => l.ToEntity()).ToDictionary(l => l.Id, l => l);

        GD.Print("Service Loaded GameDataService");
    }

    public SpecDefinition GetSpecDefinition(string specDefinitionId)
    {
        if (!specDefinitionsById.ContainsKey(specDefinitionId))
        {
            GD.Print("Before Error");
            foreach (var specDefinitionById in specDefinitionsById)
            {
                GD.Print(specDefinitionById.Key + " : " + specDefinitionById.Value.Id);
            }

            throw new IndexOutOfRangeException($"SpecDefinition {specDefinitionId} not found");
        }

        return specDefinitionsById[specDefinitionId];
    }

    public HintDef GetHintDefinition(string hintDefId)
    {
        if (!HintDefById.ContainsKey(hintDefId))
        {
            throw new IndexOutOfRangeException($"HintDefinition {hintDefId} not found");
        }

        return HintDefById[hintDefId];
    }

    public Ingredient GetIngredient(string ingredientId)
    {
        if (!ingredientsById.ContainsKey(ingredientId))
        {
            throw new IndexOutOfRangeException($"Ingredient {ingredientId} not found");
        }

        return ingredientsById[ingredientId];
    }

    private List<T> LoadAssetsRecursive<T>(string path) where T : class
    {
        List<T> assets = new List<T>();
        var fullpath = $"res://Assets/GameData/{path}";

        GD.Print($"fullpath: {fullpath}");

        using var dir = DirAccess.Open(fullpath);
        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                GD.Print("DiscoveredFile: " + fileName);
                if (fileName.EndsWith(".tres"))
                {
                    GD.Print($"loading {fileName}");
                    T res = GD.Load<T>($"{fullpath}/{fileName}");
                    assets.Add(res);
                }
                else if (fileName.EndsWith(".tres.remap"))
                {
                    // to remove ".remap" at the end
                    fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    GD.Print($"loading {fileName}");
                    T res = GD.Load<T>($"{fullpath}/{fileName}");
                    assets.Add(res);
                }

                if (dir.CurrentIsDir())
                {
                    assets.AddRange(LoadAssetsRecursive<T>($"{path}/{fileName}"));
                }

                fileName = dir.GetNext();
            }
        }
        else
        {
            GD.PrintRich($"[color=red]An error occurred when trying to access assets at {fullpath} [/color]");
        }

        return assets;
    }

    public string GetHintFor(SpecTypes specType, int summonIndex, int bossIndex)
    {
        var id = HintDef.CreateId(specType, summonIndex, bossIndex);
        return GetHintDefinition(id).Text;
    }

    internal string GetMonsterName(SummoningSpecs summoningSpecs)
    {
        var emotion = GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Emotion, summoningSpecs.Emotion.Index));
        var element = GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Element, summoningSpecs.Element.Index));
        var species = GetSpecDefinition(SpecDefinition.CreateId(SpecTypes.Species, summoningSpecs.Species.Index));

        return $"{emotion.MonsterNaming} {element.MonsterNaming} {species.MonsterNaming}";
    }
}