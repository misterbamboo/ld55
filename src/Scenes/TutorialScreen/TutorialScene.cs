using Godot;
using System;

public partial class TutorialScene : Node
{
    private const int TopCardPos = -1080 * 2;
    private const int CenterCardPos = 1080 / 2;
    private const int BottomCardPos = 1080 * 2;

    [Export] Node2D card;
    private bool fadeIn;
    private bool fadeOut;
    private float animValue;
    private float startY;
    private float endY;
    private float startAlpha;
    private float endAlpha;

    private Action animationFinishedCallback;

    private bool requestStart;

    public override void _Ready()
    {
        var color = card.Modulate;
        color.A = 0f;
        card.Modulate = color;

        TriggerFadeIn();
    }

    public override void _Process(double delta)
    {
        AnimateCard(delta);

        if (PlayerReadyToStard())
        {
            TriggerFadeOut();
        }
        else
        {
            CheckIfPlayerRequestToStart();
        }
    }

    private void CheckIfPlayerRequestToStart()
    {
        var before = requestStart;
        requestStart |= Input.IsMouseButtonPressed(MouseButton.Left) || Input.IsKeyPressed(Key.Enter) || Input.IsKeyPressed(Key.Space);

        if (before != requestStart)
        {
            GD.Print("Ready to start");
        }
    }

    private bool PlayerReadyToStard()
    {
        return !fadeIn && !fadeOut && requestStart;
    }

    private void TriggerFadeIn()
    {
        GD.Print("face in");

        animValue = 0;
        startY = BottomCardPos;
        endY = CenterCardPos;
        startAlpha = 0;
        endAlpha = 1;
        fadeIn = true;
    }

    private void TriggerFadeOut()
    {
        GD.Print("face out");

        animValue = 0;
        startY = CenterCardPos;
        endY = TopCardPos;
        startAlpha = 1;
        endAlpha = 0;
        fadeOut = true;
        animationFinishedCallback = () => ChangeScene();
    }

    private void AnimateCard(double delta)
    {
        if (!fadeIn && !fadeOut)
        {
            return;
        }

        animValue += (float)delta;
        var t = Ease.EaseIn(animValue);

        var pos = card.Position;
        pos.Y = (float)Mathf.Lerp(startY, endY, t);
        card.Position = pos;

        var color = card.Modulate;
        color.A = (float)Mathf.Lerp(startAlpha, endAlpha, t);
        card.Modulate = color;

        if (animValue >= 1)
        {
            animValue = 1;
            fadeIn = false;
            fadeOut = false;
            if (animationFinishedCallback != null)
            {
                animationFinishedCallback();
            }
        }
    }

    private void ChangeScene()
    {
        GetTree().ChangeSceneToFile("res://Scenes/start_scene.tscn");
    }
}
