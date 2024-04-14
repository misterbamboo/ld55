using Godot;

public partial class ScriptableSpecDefinition : Resource
{
    [Export] public int Index { get; set; }
    [Export] public SpecTypes SpecType { get; set; }
    [Export] public string SpecNaming { get; set; }
    [Export] public string MonsterNaming { get; set; }

    public SpecDefinition ToEntity()
    {
        return new SpecDefinition(Index, SpecType, SpecNaming, MonsterNaming);
    }
}
