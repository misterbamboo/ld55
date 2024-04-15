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
        deskManager.OnFight += ReceiveLootFromMonster;
     
        VerifyInventorySlotsOrder();
        RedrawInventoryItems();
    }

    private void ReceiveLootFromMonster(BossFight bossFight)
    {
        if(bossFight.Result != BossFight.BossFightResult.PlayerWin)
        {
            return;
        }

        var possibleLoot = gameDataService.Ingredients;
        for(var i = 0; i < bossFight.PlayerWins * 2; i++)
        {
            var ingredientIndex = rng.RandiRange(0, possibleLoot.Count());
            Inventory.AddItem(possibleLoot.ElementAt(ingredientIndex));
        }
    }

    private void HandleNewGame()
    {
        foreach (var ingredient in gameDataService.Ingredients)
        {
            Inventory.AddItem(ingredient);
            Inventory.AddItem(ingredient);
        }
        RedrawInventoryItems();

        deskManager.NewMonster(new SummoningSpecs(2.5, 2.5, 2.5));
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
        else if(sourceIndex == ArcaneFocusSlot)
        {
            deskManager.EmptyArcaneForcus();
        }

        if (sourceIndex >= Inventory.InventorySlots || destinationIndex >= Inventory.InventorySlots)
        {
            InsertInSummoningCircle();
        }
    }

    public void InsertInSummoningCircle()
    {
        var monsterSpecs = new SummoningSpecs(2.5, 2.5, 2.5);
        AddStatsForAllSlots(monsterSpecs);
        deskManager.UpdateMonsterStats(monsterSpecs);
    }

    private int ArcaneFocusSlot => Inventory.TotalSlots - 1;

    private void AddStatsForAllSlots(SummoningSpecs monsterSpecs)
    {
        for (int i = Inventory.TotalSlots - 2; i >= Inventory.TotalSlots - 6; i--)
        {
            var ingredient = Inventory.GetItemInSlot(i);
            if(ingredient == null)
            {
                continue;
            }
            monsterSpecs.AddEmotion(ingredient.EmotionValue);
            monsterSpecs.AddSpecies(ingredient.SpeciesValue);
            monsterSpecs.AddElement(ingredient.ElementValue);
        }

        GD.Print(monsterSpecs);
    }

    public void ClearSummoningBoard(SummoningSpecs monster)
    {
        Inventory.ClearSlot(20);
        Inventory.ClearSlot(21);
        Inventory.ClearSlot(22);
        Inventory.ClearSlot(23);
        Inventory.ClearSlot(24);

        RedrawInventoryItems();
        deskManager.NewMonster(new SummoningSpecs(2.5, 2.5, 2.5));
    }

    public override void _ExitTree()
    {
        deskManager.OnMonsterSummoned -= ClearSummoningBoard;
        deskManager.OnGameStart -= HandleNewGame;
    }
}

