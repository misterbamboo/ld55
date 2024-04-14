using Godot;

public partial class ScriptableHintDef : Resource
{
    [Export] public SpecTypes SpecType { get; set; }
    //(PropertyHint.Range, "0,4,")
    [Export] public int FromIndex { get; set; }
    [Export] public int ToIndex { get; set; }
    [Export] public string Text { get; set; }

    public HintDef ToEntity()
    {
        return new HintDef(SpecType, FromIndex, ToIndex, Text);
    }
}