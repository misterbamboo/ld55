using Godot;

public class ControlDragHandler
{
    private const bool DebugMode = true;

    public bool MouseHover => mouseHover;

    private bool AllowDrag { get; set; }
    private Control control;
    private bool mouseHover = false;
    private bool mouseDrag = false;
    private Vector2 mouseOffset = Vector2.Zero;

    public ControlDragHandler(Control control, bool allowDrag)
    {
        this.control = control;

        AllowDrag = allowDrag;
    }

    internal void HandleInput(InputEvent @event)
    {
        if (!mouseHover && @event is InputEventMouseMotion mouseMoveIn)
        {
            MouseInHandle(mouseMoveIn);
        }
        else if (!mouseDrag && mouseHover && @event is InputEventMouseMotion mouseMoveOut)
        {
            MouseOutHandle(mouseMoveOut);
        }
        else if (mouseHover && @event is InputEventMouseButton mouseClick)
        {
            MouseClickHandle(mouseClick);
        }

        if (mouseDrag && @event is InputEventMouseMotion mouseMoveDrag)
        {
            DragHandle(mouseMoveDrag);
        }
    }

    private void MouseInHandle(InputEventMouseMotion mouseMoveIn)
    {
        if (control.GetRect().HasPoint(mouseMoveIn.Position))
        {
            Debug("In");
            mouseHover = true;
        }
    }

    private void MouseOutHandle(InputEventMouseMotion mouseMoveOut)
    {
        if (!control.GetRect().HasPoint(mouseMoveOut.Position))
        {
            Debug("Out");
            mouseHover = false;
            mouseDrag = false;
        }
    }

    private void MouseClickHandle(InputEventMouseButton mouseClick)
    {
        if (AllowDrag)
        {
            if (!mouseDrag && mouseClick.Pressed && mouseClick.ButtonIndex == MouseButton.Left)
            {
                mouseDrag = true;
                mouseOffset = mouseClick.Position - control.Position;
                Debug($"Start drag ({mouseOffset})");
            }
            else if (mouseDrag && !mouseClick.Pressed && mouseClick.ButtonIndex == MouseButton.Left)
            {
                Debug("Stop drag");
                mouseDrag = false;
            }
        }
    }

    private void DragHandle(InputEventMouseMotion mouseMoveDrag)
    {
        if (AllowDrag)
        {
            Debug($"{mouseMoveDrag.Position} - {mouseOffset} = {mouseMoveDrag.Position - mouseOffset}");
            control.Position = mouseMoveDrag.Position - mouseOffset;
        }
    }

    private void Debug(string message)
    {
        if (DebugMode)
        {
            GD.Print(message);
        }
    }
}
