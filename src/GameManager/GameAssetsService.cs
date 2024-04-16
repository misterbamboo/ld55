using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class GameAssetsService : Node
{
    public static string Path = "/root/GameAssetsService";

    private Dictionary<string, Texture2D> gameIcons;

    public Texture2D GetSprite(string name)
    {
        if (!gameIcons.ContainsKey(name))
        {
            GD.PrintRich($"[color=red]Sprite {name} has no corresponding sprite, using default[/color]");
            return gameIcons["default"];
        }

        return gameIcons[name];
    }

    public override void _Ready()
    {
        gameIcons = LoadAssetsRecursive("").ToDictionary(s => ExtractNameFromFilePath(s.ResourcePath), s => s);

        GD.Print("Service Loaded GameAssetsService");
    }

    private string ExtractNameFromFilePath(string path)
    {
        return System.IO.Path.GetFileNameWithoutExtension(path);
    }

    private List<Texture2D> LoadAssetsRecursive(string path)
    {
        List<Texture2D> assets = new List<Texture2D>();
        var fullpath = $"res://Assets/Icons{path}";

        GD.Print($"fullpath: {fullpath}");

        using var dir = DirAccess.Open(fullpath);
        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                GD.Print("Discovered icons: " + fileName);
                if (fileName.EndsWith(".png"))
                {
                    GD.Print($"loading {fileName}");
                    Texture2D res = ResourceLoader.Load<Texture2D>($"{fullpath}/{fileName}");
                    assets.Add(res);
                }

                if (fileName.EndsWith(".import"))
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    GD.Print($"loading {fileName}");
                    Texture2D res = ResourceLoader.Load<Texture2D>($"{fullpath}/{fileName}");
                    assets.Add(res);
                }

                if (dir.CurrentIsDir())
                {
                    assets.AddRange(LoadAssetsRecursive($"{path}/{fileName}"));
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
