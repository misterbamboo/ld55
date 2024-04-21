using Godot;
using System;

public partial class DragZoneUI : Control
{
    private ControlDragHandler controlDragHandler;
    private DeskManager deskManager;

    public bool CanDrag { get; private set; } = true;

    public override void _Ready()
    {
        deskManager = GetNode<DeskManager>(DeskManager.Path);

        // Events that allow drag
        deskManager.OnArcaneFocusDragEnd += OnCanDrag;
        deskManager.OnItemDropped += OnCanDrag;

        // Events that disallow drag
        deskManager.OnArcaneFocusDragStart += OnCanNOTDrag;
        deskManager.OnItemGrabbed += OnCanNOTDrag;

        controlDragHandler = new ControlDragHandler(this, true);
    }

    private void OnCanDrag()
    {
        CanDrag = true;
    }

    private void OnCanNOTDrag()
    {
        CanDrag = false;
    }

    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        if (CanDrag)
        {
            controlDragHandler.HandleInput(@event);
        }
        else
        {
            controlDragHandler.Reset();
        }
    }
}
