using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public partial class LogbookSceneUI : Node
{
    [Export] private RichTextLabel logbookTextLabel;

    private DeskManager deskManager;

    private Stack<BossFight> lastBossFights = new Stack<BossFight>();

    public override void _Ready()
    {
        deskManager = GetNode<DeskManager>(DeskManager.Path);
        deskManager.OnFightCompleted += DeskManager_OnFightCompleted;
    }

    public override void _Process(double delta)
    {
    }

    private void DeskManager_OnFightCompleted(BossFight bossFigth)
    {
        lastBossFights.Push(bossFigth);
        UpdateLogBook();
    }

    private void UpdateLogBook()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var lastBossFight in lastBossFights)
        {
            sb.AppendLine($"{lastBossFight.Player}");
        }

        logbookTextLabel.Text = sb.ToString();
    }
}
