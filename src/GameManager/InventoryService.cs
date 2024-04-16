using Godot;
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
        summoningStats = GetNode<SummoningStatsUI>("../SummoningStatsUI");
        Inventory = new Inventory();

        deskManager.OnMonsterSummoned += ClearSummoningBoard;
        deskManager.OnGameStart += HandleNewGame;
        deskManager.OnFightCompleted += ReceiveLootFromMonster;

        VerifyInventorySlotsOrder();
        RedrawInventoryItems();
    }

    private void ReceiveLootFromMonster(BossFight bossFight)
    {
        if (bossFight.Result != BossFight.BossFightResult.PlayerWin)
        {
            return;
        }

        var possibleLoot = gameDataService.Ingredients;
        for (var i = 0; i < bossFight.PlayerWins * 2; i++)
        {
            var ingredientIndex = rng.RandiRange(0, possibleLoot.Count() - 1);
            Inventory.AddItem(possibleLoot.ElementAt(ingredientIndex));
        }

        RedrawInventoryItems();
    }

    private void HandleNewGame()
    {
        foreach (var ingredient in gameDataService.Ingredients)
        {
            Inventory.AddItem(ingredient);
            Inventory.AddItem(ingredient);
        }
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

    public void Swap(int sourceIndex, int destinationIndex)
    {
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
            GD.Print($"source: {sourceIndex}, destination: {destinationIndex}, focus: {ArcaneFocusSlot}");
            InsertInSummoningCircle();
        }
    }

    public bool IsOnSummoningBoard(int index)
    {
        return index >= 20 && index <= 24;
    }

    public void InsertInSummoningCircle()
    {
        var ingredientsOnBoard = Inventory.Items().Skip(Inventory.InventorySlots+1).Take(5);
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

