using System;
using System.Collections.Generic;
using System.Linq;

public class Inventory
{
    private List<Ingredient> Ingredients { get; }

    public int TotalSlots { get;  }
    public int InventorySlots { get; }

    public Inventory()
    {
        TotalSlots = 26;
        InventorySlots = 20;

        Ingredients = EmptyInventory(TotalSlots).ToList();
    }

    public IEnumerable<Ingredient> Items()
    {
        return Ingredients;
    }

    public IEnumerable<Ingredient> NonVoidItems()
    {
        return Ingredients.Where(ingredient => !ingredient.IsVoid);
    }

    public Ingredient GetItem(string ingredientId)
    {
        if (ItemIndex(ingredientId) != -1)
        {
            return Ingredients[ItemIndex(ingredientId)];
            
        }

        // TODO: Add inventory name here
        throw new Exception($"No {ingredientId} in inventory ");
    }

    public int ItemIndex(string IngredientId)
    {
        return Ingredients.FindIndex(0, ingredient => ingredient.Id == IngredientId);
    }

    public void AddItem(Ingredient ingredient)
    {
        if (CanAddItems())
        {
            Ingredients[FirstFreeSlot] = ingredient;
            return;
        }

        // TODO: Add inventory name here
        throw new Exception("Inventory Full");
    }

    public int FirstFreeSlot => Ingredients.Take(InventorySlots).ToList().FindIndex(0, ingredient => ingredient.IsVoid);

    public bool CanAddItems()
    {
        return FirstFreeSlot != -1;
    }

    public static IEnumerable<Ingredient> EmptyInventory(int slots)
    {
        for (int i = 0; i < slots; i++)
        {
            yield return Ingredient.Void();
        }
    }

    public void ClearSlot(int index)
    {
        Ingredients[index] = Ingredient.Void();
    }

    public void SwapItems(int index1, int index2)
    {
        if (index2IsATrailingVoidItem(index2)) return;

        var temp = Ingredients[index1];
        Ingredients[index1] = Ingredients[index2];
        Ingredients[index2] = temp;
    }

    private bool index2IsATrailingVoidItem(int index2)
    {
        var index2IsAVoidItem = Ingredients[index2].IsVoid;
        if (!index2IsAVoidItem) return false;

        var OnlyVoidItemsAreAfterIndex2 = Ingredients.Skip(index2 + 1).All(ingredient => ingredient.IsVoid);

        return OnlyVoidItemsAreAfterIndex2;
    }
}

