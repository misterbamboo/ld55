using Godot;
using System;

public partial class EasingTestScript : Node
{
    [Export] private VSlider vSlider;
    [Export] private HSlider hSlider;
    [Export] private ItemList itemList;

    private EasingModes EasingMode { get; set; }

    public override void _Ready()
    {
        var names = Enum.GetNames<EasingModes>();
        foreach (var name in names)
        {
            itemList.AddItem(name);
        }
    }

    public override void _Process(double delta)
    {
        var ratio = hSlider.Value / hSlider.MaxValue;
        double t = ApplyEase(ratio);
        vSlider.Value = t * vSlider.MaxValue;
    }

    private double ApplyEase(double ratio)
    {
        switch (EasingMode)
        {
            case EasingModes.Spike:
                return Ease.Spike(ratio);
            case EasingModes.None:
            default:
                return ratio;
        }
    }

    public void ItemSelected(int index)
    {
        var name = itemList.GetItemText(index);
        EasingMode = Enum.Parse<EasingModes>(name);
    }
}

public enum EasingModes
{
    None,
    Spike
}