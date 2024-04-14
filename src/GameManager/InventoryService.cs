using Godot;
using System;
using System.Linq;

public partial class InventoryService : Node
{
    private GameDataService gameDataService;
    [Export] private Godot.Collections.Array<IngredientSlotUI> InventorySlots;
    [Export] private SummoningStatsUI summoningStats;

    private Inventory Inventory;

    public override void _Ready()
    {
        gameDataService = GetNode<GameDataService>(GameDataService.Path);
        summoningStats = GetNode<SummoningStatsUI>("../SummoningStatsUI");
        Inventory = new Inventory();

        foreach (var ingredient in gameDataService.Ingredients)
        {
            Inventory.AddItem(ingredient);
            Inventory.AddItem(ingredient);
        }
     
        VerifyInventorySlotsOrder();
        RedrawInventoryItems();
    }

    private void VerifyInventorySlotsOrder()
    {
        if (Inventory.TotalSlots != InventorySlots.Count)
        {
            throw new System.Exception("missing inventory slots");
        }

        for (int i = 0; i < Inventory.TotalSlots; i++)
        {
            if (InventorySlots[i].Index != i)
            {
                throw new System.Exception($"inventory slot misconfigured at index {i} or {InventorySlots[i].Index}");
            }
        }
    }

    private void RedrawInventoryItems()
    {
        for (int i = 0; i < Inventory.TotalSlots; i++)
        {
            var slot = InventorySlots[i];
            var itemInSlot = Inventory.Items().ElementAt(i);
            slot.Init(itemInSlot, i, this);
        }
    }

    public void Swap(int index1, int index2)
    {
        Inventory.SwapItems(index1, index2);
        RedrawInventoryItems();

        if (index1 >= Inventory.InventorySlots || index2 >= Inventory.InventorySlots)
        {
            CountProperties();
        }
    }

    public void CountProperties()
    {
        GD.Print("Something in special slots");
        var monsterSpecs = new SummoningSpecs(2.5,2.5,2.5);

        for(int i = Inventory.TotalSlots-2; i >= Inventory.TotalSlots-6; i--)
        {
            var ingredient = Inventory.GetItemInSlot(i);
            monsterSpecs.AddEmotion(ingredient.Emotion);
            monsterSpecs.AddSpecies(ingredient.Species);
            monsterSpecs.AddElement(ingredient.Element);
        }

        summoningStats.SetBars(monsterSpecs);

        if (!Inventory.GetItemInSlot(Inventory.TotalSlots - 1).IsVoid)
        {
            GD.Print("Something in the mixer");
            var previewSpecs = new SummoningSpecs(monsterSpecs.Emotion.Value, monsterSpecs.Element.Value, monsterSpecs.Species.Value);
            var ingredient = Inventory.GetItemInSlot(Inventory.TotalSlots - 1);
            previewSpecs.AddEmotion(ingredient.Emotion);
            previewSpecs.AddSpecies(ingredient.Species);
            previewSpecs.AddElement(ingredient.Element);

            summoningStats.SetPreviews(previewSpecs);
        }
    }

    public void ClearSummoningBoard()
    {
        Inventory.ClearSlot(20);
        Inventory.ClearSlot(21);
        Inventory.ClearSlot(22);
        Inventory.ClearSlot(23);
        Inventory.ClearSlot(24);

        RedrawInventoryItems();
    }
}

