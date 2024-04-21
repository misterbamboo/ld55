using Godot;
using System;

public class ControlDragHandler
{
    private const bool DebugMode = true;

    public event Action OnStartDraggin;
    public event Action OnEndDraggin;

    public event Action OnStartMouseHover;
    public event Action OnEndMouseHover;

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

    internal void Reset()
    {
        if (mouseDrag)
        {
            mouseDrag = false;
            OnEndDraggin?.Invoke();
        }

        if (mouseHover)
        {
            mouseHover = false;
            OnEndMouseHover?.Invoke();
        }
    }

    internal void HandleInput(InputEvent @event)
    {
        if (!mouseHover && @event is InputEventMouseMotion mouseMoveIn)
        {
            MouseInHandle(mouseMoveIn);
        }

        if (!mouseDrag && mouseHover && @event is InputEventMouseMotion mouseMoveOut)
        {
            MouseOutHandle(mouseMoveOut);
        }

        if (mouseHover && @event is InputEventMouseButton mouseClick)
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
        if (control.GetGlobalRect().HasPoint(mouseMoveIn.GlobalPosition))
        {
            Debug("In");
            mouseHover = true;
            OnStartMouseHover?.Invoke();
        }
    }

    private void MouseOutHandle(InputEventMouseMotion mouseMoveOut)
    {
        if (!control.GetGlobalRect().HasPoint(mouseMoveOut.GlobalPosition))
        {
            Debug("Stop drag");
            mouseDrag = false;
            OnEndDraggin?.Invoke();

            Debug("Out");
            mouseHover = false;
            OnEndMouseHover?.Invoke();
        }
    }

    private void MouseClickHandle(InputEventMouseButton mouseClick)
    {
        if (AllowDrag)
        {
            if (!mouseDrag && mouseClick.Pressed && mouseClick.ButtonIndex == MouseButton.Left)
            {
                mouseDrag = true;
                mouseOffset = mouseClick.GlobalPosition - control.GlobalPosition;
                Debug($"Start drag ({mouseOffset})");
                OnStartDraggin?.Invoke();
            }
            else if (mouseDrag && !mouseClick.Pressed && mouseClick.ButtonIndex == MouseButton.Left)
            {
                Debug("Stop drag");
                mouseDrag = false;
                OnEndDraggin?.Invoke();
            }
        }
    }

    private void DragHandle(InputEventMouseMotion mouseMoveDrag)
    {
        if (AllowDrag)
        {
            Debug($"{mouseMoveDrag.GlobalPosition} - {mouseOffset} = {mouseMoveDrag.GlobalPosition - mouseOffset}");
            control.GlobalPosition = mouseMoveDrag.GlobalPosition - mouseOffset;
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
