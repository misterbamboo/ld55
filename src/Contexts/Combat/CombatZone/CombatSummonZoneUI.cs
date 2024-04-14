
using Godot;

public partial class CombatSummonZoneUI : Control
{
    private ControlDragHandler DragHandler { get; set; }
    public CombatSummonZoneUI()
    {
        DragHandler = new ControlDragHandler(this, allowDrag: true);
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