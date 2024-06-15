using Godot;

public partial class IngredientSlotUI : Control
{
    private GameAssetsService gameAssetsService;
    private DeskManager deskManager;
    private InventoryService inventory;
    private TextureRect Icon;

    private Ingredient ingredient;

    [Export] private int index;

    public bool lastMousePressed { get; set; }
    private Vector2 lastMousePos = Vector2.Inf;
    private bool mouseHover;

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

    public override void _Input(InputEvent @event)
    {
        if (mouseHover && @event is InputEventMouseButton mButtonEvent)
        {
            lastMousePressed = mButtonEvent.Pressed;
            if (mButtonEvent.Pressed)
            {
                lastMousePos = Vector2.Inf;
            }
        }
        else if (@event is InputEventMouse mEvent)
        {
            CheckMouseOver(mEvent);
            CheckForceDrag(mEvent);
        }
    }

    private void CheckMouseOver(InputEventMouse mEvent)
    {
        if (GetGlobalRect().HasPoint(mEvent.GlobalPosition))
        {
            mouseHover = true;
        }
        else
        {
            mouseHover = false;
        }
    }

    private void CheckForceDrag(InputEventMouse mEvent)
    {
        bool leftDown = (mEvent.ButtonMask & MouseButtonMask.Left) == MouseButtonMask.Left;
        if (lastMousePressed && leftDown)
        {
            if (lastMousePos == Vector2.Inf)
            {
                lastMousePos = mEvent.Position;

                var variantValue = InternalGetDragData(out Control control);
                if (control != null)
                {
                    ForceDrag(variantValue, control);
                }
            }
        }
    }

    public void Init(Ingredient ingredient, int index, InventoryService inventory)
    {
        this.ingredient = ingredient;
        this.inventory = inventory;
        if (index != this.index)
        {
            throw new System.Exception($"misconfigured item slot at index {index} or {this.index}");
        }
        Icon.Texture = gameAssetsService.GetSprite(ingredient.Icon);
    }

    public override Variant _GetDragData(Vector2 position)
    {
        Control control;
        var variantToReturn = InternalGetDragData(out control);
        if (control != null)
        {
            SetDragPreview(control);
        }
        return variantToReturn;
    }

    private Variant InternalGetDragData(out Control control)
    {
        if (inventory.IsEmptySlot(index))
        {
            control = null;
            return default;
        }

        deskManager.ItemGrabbed();

        var rect = new TextureRect();
        rect.Texture = Icon.Texture;
        rect.CustomMinimumSize = new Vector2(80, 80);
        rect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        rect.StretchMode = TextureRect.StretchModeEnum.Scale;
        control = rect;

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
