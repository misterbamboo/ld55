using Godot;

public partial class IngredientSlotUI : Control
{
	private GameAssetsService gameAssetsService;
    private DeskManager deskManager;
    private InventoryService inventory;
	private TextureRect Icon;

	private Ingredient ingredient;

    [Export] private int index;

    public int Index => index;
	
    public override void _Ready()
	{
        Icon = GetNode<TextureRect>("Icon");
		gameAssetsService = GetNode<GameAssetsService>(GameAssetsService.Path);
        deskManager = GetNode<DeskManager>(DeskManager.Path);
    }

	public override void _Process(double delta)
	{
	}

	public void Init(Ingredient ingredient, int index, InventoryService inventory)
	{
        this.ingredient = ingredient;
        this.inventory = inventory;
        if(index != this.index)
        {
            throw new System.Exception($"misconfigured item slot at index {index} or {this.index}");
        }
        Icon.Texture = gameAssetsService.GetSprite(ingredient.Icon);
    }

    public override Variant _GetDragData(Vector2 position)
    {
        deskManager.ItemGrabbed();

        var rect = new TextureRect();
        rect.Texture = Icon.Texture;
        rect.CustomMinimumSize = new Vector2(80, 80);
        rect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        rect.StretchMode = TextureRect.StretchModeEnum.Scale;

        SetDragPreview(rect);
        return index;
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return data.AsInt32() != index;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        inventory.Swap(data.AsInt32(), index);
        deskManager.ItemDropped();
    }
}
