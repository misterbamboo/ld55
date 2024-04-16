using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class GameAssetsService : Node
{
    public static string Path = "/root/GameAssetsService";

    private Dictionary<string, Texture2D> gameIcons = new Dictionary<string, Texture2D>();

    public Texture2D GetSprite(string name)
    {
        if (!gameIcons.ContainsKey(name))
        {
            GD.PrintRich($"[color=red]Sprite {name} has no corresponding sprite, using default[/color]");
            foreach (var gameIcon in gameIcons)
            {
                GD.Print(gameIcon.Key + ": " + gameIcon.Value.ResourcePath);
            }
            return gameIcons["default"];
        }

        return gameIcons[name];
    }

    public override void _Ready()
    {
        gameIcons = LoadAssetsRecursive("").ToDictionary(s => ExtractNameFromFilePath(s.ResourcePath), s => s);

        GD.Print("Service Loaded GameAssetsService");
    }

    private HashSet<string> fuck = new HashSet<string>();
    private string ExtractNameFromFilePath(string path)
    {
        var name = string.Empty;
        if (path.EndsWith(".png"))
        {
            name = System.IO.Path.GetFileNameWithoutExtension(path);
        }
        else if (path.EndsWith(".png.import"))
        {
            name = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileNameWithoutExtension(path));
        }

        if (fuck.Contains(name))
        {
            name += ".fuck";
        }
        else
        {
            fuck.Add(name);
        }

        return name;
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
                    GD.Print($"loading png? {fileName}");
                    Texture2D res = ResourceLoader.Load<Texture2D>($"{fullpath}/{fileName}");
                    assets.Add(res);
                }
                else if (fileName.EndsWith(".png.import"))
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    GD.Print($"loading png/import? {fileName}");
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
