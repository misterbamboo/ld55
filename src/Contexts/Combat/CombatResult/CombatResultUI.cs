using Godot;

public partial class CombatResultUI : Node
{
    [Export] public RichTextLabel RichTextLabel { get; set; }
    private DeskManager DeskManager { get; set; }

    public override void _Ready()
    {
        RichTextLabel.Text = string.Empty;
        DeskManager = GetNode<DeskManager>(DeskManager.Path);
        DeskManager.OnFightCompleted += DeskManager_OnFight;
    }

    private void DeskManager_OnFight(BossFight bossFigth)
    {
        var adjective = GetDisplayAdjective(bossFigth.Result);
        var line1 = $"You {adjective}. {bossFigth.Hint}";
        var line2 = $"specs here ...";
        RichTextLabel.Text = $"{line1} {System.Environment.NewLine} {line2}";
    }

    private static string GetDisplayAdjective(BossFight.BossFightResult bossFigthResult)
    {
        switch (bossFigthResult)
        {
            case BossFight.BossFightResult.Draw:
                return "draw";
            case BossFight.BossFightResult.PlayerWin:
                return "win";
            case BossFight.BossFightResult.PlayerLose:
                return "lose";
            case BossFight.BossFightResult.Undefined:
            default:
                return "_";
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        DeskManager.OnFightCompleted -= DeskManager_OnFight;
    }
}
