using Godot;

public class MonsterCardUIDragHandler
{
    private MonsterCardUI monsterCardUI;
    private bool mouseHover = false;
    private bool mouseDrag = false;
    private Vector2 mouseOffset = Vector2.Zero;

    public MonsterCardUIDragHandler(MonsterCardUI monsterCardUI)
    {
        this.monsterCardUI = monsterCardUI;
    }

    internal void HandleInput(InputEvent @event)
    {
        if (!mouseHover && @event is InputEventMouseMotion mouseMoveIn)
        {
            if (monsterCardUI.GetRect().HasPoint(mouseMoveIn.Position))
            {
                // GD.Print("In");
                mouseHover = true;
            }
        }
        else if (!mouseDrag && mouseHover && @event is InputEventMouseMotion mouseMoveOut)
        {
            if (!monsterCardUI.GetRect().HasPoint(mouseMoveOut.Position))
            {
                // GD.Print("Out");
                mouseHover = false;
                mouseDrag = false;
            }
        }
        else if (mouseHover && @event is InputEventMouseButton mouseClick)
        {
            if (!mouseDrag && mouseClick.Pressed && mouseClick.ButtonIndex == MouseButton.Left)
            {
                mouseDrag = true;
                mouseOffset = mouseClick.Position - monsterCardUI.Position;
                // GD.Print($"Start drag ({mouseOffset})");
            }
            else if (mouseDrag && !mouseClick.Pressed && mouseClick.ButtonIndex == MouseButton.Left)
            {
                // GD.Print("Stop drag");
                mouseDrag = false;
            }
        }

        if (mouseDrag && @event is InputEventMouseMotion mouseMoveDrag)
        {
            // GD.Print($"{mouseMoveDrag.Position} - {mouseOffset} = {mouseMoveDrag.Position - mouseOffset}");
            monsterCardUI.Position = mouseMoveDrag.Position - mouseOffset;
        }
    }
}
