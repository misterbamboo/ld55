using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerHand : Node
{
    public static PlayerHand Instance { get; private set; }

    public List<MonsterCardUI> HoldCards { get; set; } = new List<MonsterCardUI>();

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _Process(double delta)
    {
    }

    internal void PutInHand(MonsterCardUI card)
    {
        if (!HoldCards.Contains(card))
        {
            HoldCards.Add(card);
            GD.Print($"Added Card: {HoldCards.Count} in hand");
        }
    }

    internal void RemoveFromHand(MonsterCardUI card)
    {
        if (HoldCards.Contains(card))
        {
            HoldCards.Remove(card);
            GD.Print($"Removed Card: {HoldCards.Count} in hand");
        }
    }

    internal MonsterCardUI? FirstCardInHand()
    {
        return HoldCards.FirstOrDefault();
    }
}
