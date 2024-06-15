using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InventoryService : Node
{
    private GameDataService gameDataService;
    private DeskManager deskManager;
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    [Export] private Godot.Collections.Array<IngredientSlotUI> InventorySlots;
    [Export] private SummoningStatsUI summoningStats;

    private Inventory Inventory;

    public override void _Ready()
    {
        deskManager = GetNode<DeskManager>(DeskManager.Path);
        gameDataService = GetNode<GameDataService>(GameDataService.Path);
        summoningStats = GetNode<SummoningStatsUI>(SummoningStatsUI.Path);
        Inventory = new Inventory();

        deskManager.OnMonsterSummoned += ClearSummoningBoard;
        deskManager.OnGameStart += HandleNewGame;
        deskManager.OnFightCompleted += ReceiveLootFromMonster;

        VerifyInventorySlotsOrder();
        RedrawInventoryItems();
    }

    private void ReceiveLootFromMonster(BossFight bossFight)
    {
        if (bossFight.Result == BossFight.BossFightResult.PlayerWin)
        {
            RegularWinLoot(bossFight);
        }
        else
        {
            MinimumLoot();
        }
    }

    private void RegularWinLoot(BossFight bossFight)
    {
        var possibleLoot = gameDataService.Ingredients;
        for (var i = 0; i < bossFight.PlayerWins * 2; i++)
        {
            var ingredientIndex = rng.RandiRange(0, possibleLoot.Count() - 1);
            Inventory.AddItem(possibleLoot.ElementAt(ingredientIndex));
        }

        RedrawInventoryItems();
    }

    private void MinimumLoot()
    {
        while (!InventoryItemsAllowToPlay())
        {
            Ingredient ingredient = GetLeastPresentSpecIngredientInInventoryLoot();
            Inventory.AddItem(ingredient);
        }
    }

    private bool InventoryItemsAllowToPlay()
    {
        var elementRawSum = Inventory.Items().Sum(i => i.Element);
        var elementAbsSum = Inventory.Items().Sum(i => Math.Abs(i.Element));
        if (elementRawSum < 1 && elementAbsSum < 1) return false;

        var speciesRawSum = Inventory.Items().Sum(i => i.Species);
        var speciesAbsSum = Inventory.Items().Sum(i => Math.Abs(i.Species));
        if (speciesRawSum < 1 && speciesAbsSum < 1) return false;

        var emotionRawSum = Inventory.Items().Sum(i => i.Emotion);
        var emotionAbsSum = Inventory.Items().Sum(i => Math.Abs(i.Emotion));
        if (emotionRawSum < 1 && emotionAbsSum < 1) return false;

        return true;
    }

    private Ingredient GetLeastPresentSpecIngredientInInventoryLoot()
    {
        var emotionSumInInventory = Inventory.Items().Sum(i => Math.Abs(i.Emotion));
        var elementSumInInventory = Inventory.Items().Sum(i => Math.Abs(i.Element));
        var speciesSumInInventory = Inventory.Items().Sum(i => Math.Abs(i.Species));

        double smallestSum = double.MaxValue;
        SpecTypes smallestSpec = SpecTypes.Species;

        if (emotionSumInInventory < smallestSum)
        {
            smallestSum = emotionSumInInventory;
            smallestSpec = SpecTypes.Emotion;
        }

        if (elementSumInInventory < smallestSum)
        {
            smallestSum = elementSumInInventory;
            smallestSpec = SpecTypes.Element;
        }

        if (speciesSumInInventory < smallestSum)
        {
            smallestSum = speciesSumInInventory;
            smallestSpec = SpecTypes.Species;
        }

        var candidatesIngredients = GetIngredientsRitchIn(smallestSpec);
        var ingredient = GetRandomIngredient(candidatesIngredients);
        return ingredient;
    }

    private void HandleNewGame()
    {
        for (var i = 0; i < 8; i++)
        {
            IEnumerable<Ingredient> candidateIngredients = gameDataService.Ingredients;
            if (i == 0)
                candidateIngredients = GetIngredientsRitchIn(SpecTypes.Element);
            if (i == 1)
                candidateIngredients = GetIngredientsRitchIn(SpecTypes.Emotion);
            if (i == 2)
                candidateIngredients = GetIngredientsRitchIn(SpecTypes.Species);

            var ingredient = GetRandomIngredient(candidateIngredients);
            Inventory.AddItem(ingredient);
        }

        RedrawInventoryItems();
    }

    private Ingredient GetRandomIngredient(IEnumerable<Ingredient> candidateIngredients)
    {
        var count = candidateIngredients.Count();
        var item = rng.RandiRange(0, count - 1);
        return candidateIngredients.ElementAt(item);
    }

    private IEnumerable<Ingredient> GetIngredientsRitchIn(SpecTypes spec)
    {
        switch (spec)
        {
            case SpecTypes.Emotion:
                return gameDataService.Ingredients.Where(i => Math.Abs(i.Emotion) > 0).ToArray();
            case SpecTypes.Element:
                return gameDataService.Ingredients.Where(i => Math.Abs(i.Element) > 0).ToArray();
            case SpecTypes.Species:
                return gameDataService.Ingredients.Where(i => Math.Abs(i.Species) > 0).ToArray();
            default:
                return gameDataService.Ingredients;
        }
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

    public bool IsEmptySlot(int sourceIndex)
    {
        var itemInSlot = Inventory.Items().ElementAt(sourceIndex);
        return itemInSlot.IsVoid;
    }

    public void Swap(int sourceIndex, int destinationIndex)
    {
        GD.Print($"source: {sourceIndex}, destination: {destinationIndex}, focus: {ArcaneFocusSlot}");
        Inventory.SwapItems(sourceIndex, destinationIndex);

        RedrawInventoryItems();

        if (destinationIndex == ArcaneFocusSlot)
        {
            deskManager.FillArcaneForcus(Inventory.GetItemInSlot(ArcaneFocusSlot));
        }
        else if (sourceIndex == ArcaneFocusSlot)
        {
            deskManager.EmptyArcaneForcus();
        }

        if (IsOnSummoningBoard(sourceIndex) || IsOnSummoningBoard(destinationIndex))
        {
            InsertInSummoningCircle();
        }
    }

    public bool IsOnSummoningBoard(int index)
    {
        return index >= 20 && index <= 24;
    }

    public void InsertInSummoningCircle()
    {
        var ingredientsOnBoard = Inventory.Items().Skip(Inventory.InventorySlots).Take(5);
        deskManager.UpdateIngredientsOnSummoningBoard(ingredientsOnBoard);
    }

    private int ArcaneFocusSlot => Inventory.TotalSlots - 1;

    public void ClearSummoningBoard(SummoningSpecs monster)
    {
        Inventory.ClearSlot(20);
        Inventory.ClearSlot(21);
        Inventory.ClearSlot(22);
        Inventory.ClearSlot(23);
        Inventory.ClearSlot(24);

        RedrawInventoryItems();
    }

    public override void _ExitTree()
    {
        deskManager.OnMonsterSummoned -= ClearSummoningBoard;
        deskManager.OnGameStart -= HandleNewGame;
    }
}

