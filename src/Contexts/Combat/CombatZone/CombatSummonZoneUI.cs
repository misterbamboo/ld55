
using Godot;

public partial class CombatSummonZoneUI : Control
{
    private ControlDragHandler DragHandler { get; set; }
    public CombatSummonZoneUI()
    {
        DragHandler = new ControlDragHandler(this, allowDrag: false);
    }

    private MonsterCardUI playerStickCard;

    public override void _Ready()
    {
        DragHandler.OnStartMouseHover += DragHandler_OnStartMouseHover;
        DragHandler.OnEndMouseHover += DragHandler_OnEndMouseHover;
    }

    private void DragHandler_OnStartMouseHover()
    {
        var card = PlayerHand.Instance.FirstCardInHand();
        if (card == null)
        {

        }

        card.TriggerStickToAnim(Position);
    }

    private void DragHandler_OnEndMouseHover()
    {
    }

    public override void _Process(double delta)
    {
        CheckCardOver();
    }

    public override void _Input(InputEvent @event)
    {
        DragHandler.HandleInput(@event);
    }

    private void CheckCardOver()
    {
        //if (DragHandler.MouseHover)
        //{
        //    GD.Print("ZoneOver");
        //}
        //else
        //{
        //    GD.Print("Zone OUT");
        //}
    }
}