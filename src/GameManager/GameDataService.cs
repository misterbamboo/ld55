using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using static Godot.OpenXRHand;

public partial class GameDataService : Node
{
    public static string Path = "/root/GameDataService";

    private Dictionary<string, Ingredient> ingredientsById;
    private Dictionary<string, SpecDefinition> specDefinitionsById;
    private Dictionary<string, HintDef> HintDefById;

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
                if (System.IO.Path.GetExtension(fileName) == ".tres")
                {
                    GD.Print($"loading {fileName}");
                    T res = ResourceLoader.Load<T>($"{fullpath}/{fileName}");
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
}
