using System;

public class HintDef
{
    public HintDef(SpecTypes specType, int fromIndex, int toIndex, string text)
    {
        Id = $"{specType}_{fromIndex}_{toIndex}";
        SpecType = specType;
        FromIndex = fromIndex;
        ToIndex = toIndex;
        Text = text;
    }

    public string Id { get; set; }
    public int MyProperty { get; set; }
    public SpecTypes SpecType { get; }
    public int FromIndex { get; }
    public int ToIndex { get; }
    public string Text { get; }

    internal static string CreateId(SpecTypes specType, int playerIndex, int bossIndex)
    {
        return $"{specType}_{playerIndex}_{bossIndex}";
    }
}
