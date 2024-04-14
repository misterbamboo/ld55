using System;

public class SpecDefinition
{
    public SpecDefinition(int index, SpecTypes specType, string specNaming, string monsterNaming)
    {
        Id = CreateId(specType, index);
        Index = index;
        SpecType = specType;
        SpecNaming = specNaming;
        MonsterNaming = monsterNaming;
    }

    public string Id { get; }
    public int Index { get; }
    public SpecTypes SpecType { get; }
    public string SpecNaming { get; }
    public string MonsterNaming { get; }
    public int Value { get; }

    internal static string CreateId(SpecTypes specType, int index)
    {
        return $"{specType}_{index}";
    }

    internal static SpecDefinition Empty()
    {
        return new SpecDefinition(0, SpecTypes.Element, string.Empty, string.Empty);
    }
}

public enum SpecTypes
{
    Emotion,
    Element,
    Species
}
