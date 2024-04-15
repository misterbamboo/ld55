
using Godot;
using System;

public partial class CombatSummonZoneUI : Control
{
    public event Action<MonsterCardUI> OnReadyToFight;

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
            return;
        }

        playerStickCard = card;
        playerStickCard.TriggerStickToAnim(GlobalPosition + new Vector2(2, 2), StickAnimEnded);
    }

    private void StickAnimEnded()
    {
        if (playerStickCard != null)
        {
            OnReadyToFight?.Invoke(playerStickCard);
        }
    }

    private void DragHandler_OnEndMouseHover()
    {
        if (playerStickCard == null)
        {
            return;
        }

        playerStickCard.ReleaseStickToAnim();
        playerStickCard = null;
    }

    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        DragHandler.HandleInput(@event);
    }
}