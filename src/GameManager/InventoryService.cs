using Godot;
using System.Linq;

public partial class InventoryService : Node
{
    public const string Path = "/root/InventoryService";

    [Export] private Godot.Collections.Array<IngredientSlotUI> InventorySlots;

    private Inventory Inventory;

    public override void _Ready()
    {
        Inventory = new Inventory();

        Inventory.AddItem(new Ingredient("ingredient_sappling_root", "Sappling Root", "ingredient_sappling_root", "ingredient_sappling_root", 1, 0, 0));
        Inventory.AddItem(new Ingredient("ingredient_sappling_root", "Sappling Root", "ingredient_sappling_root", "ingredient_sappling_root", 1, 0, 0));
        Inventory.AddItem(new Ingredient("ingredient_sappling_root", "Sappling Root", "ingredient_sappling_root", "ingredient_sappling_root", 1, 0, 0));
        Inventory.AddItem(new Ingredient("ingredient_sappling_root", "Sappling Root", "ingredient_sappling_root", "ingredient_sappling_root", 1, 0, 0));
        Inventory.AddItem(new Ingredient("ingredient_sappling_root", "Sappling Root", "ingredient_sappling_root", "ingredient_sappling_root", 1, 0, 0));
        
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
    }

    public void CancelMove()
    {
        RedrawInventoryItems();
    }
}

