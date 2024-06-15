using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class LogbookSceneUI : Node
{
    [Export] private RichTextLabel logbookTextLabel;

    [Export] private Node[] arrowsParentNodes;
    private Node2D[] arrowAndSpec2DNodes;

    private GameDataService gameDataService;
    private DeskManager deskManager;
    private RandomNumberGenerator rand;
    private Stack<BossFight> lastBossFights = new Stack<BossFight>();
    private List<BossFightWinDetails> WinsDetails { get; } = new List<BossFightWinDetails>();

    public override void _Ready()
    {
        gameDataService = GetNode<GameDataService>(GameDataService.Path);
        deskManager = GetNode<DeskManager>(DeskManager.Path);
        rand = new RandomNumberGenerator();
        rand.Randomize();
        deskManager.OnFightCompleted += DeskManager_OnFightCompleted;

        ClearDefaultLogbookText();
        DiscoverHint2DNodes();
        HideHints();
    }

    private void ClearDefaultLogbookText()
    {
        logbookTextLabel.Text = string.Empty;
    }

    private void DiscoverHint2DNodes()
    {
        List<Node2D> buildArrowNodes = new List<Node2D>();
        foreach (var arrowParentNode in arrowsParentNodes)
        {
            var children = arrowParentNode.GetChildren().OfType<Node2D>();
            buildArrowNodes.AddRange(children);
        }
        arrowAndSpec2DNodes = buildArrowNodes.ToArray();
    }

    private void HideHints()
    {
        foreach (var arrowNode in arrowAndSpec2DNodes)
        {
            arrowNode.Visible = false;
        }
    }

    bool lastState = false;

    public override void _Process(double delta)
    {
        var newState = Input.IsKeyLabelPressed(Key.R);
        if (lastState != newState)
        {
            lastState = newState;
            if (newState)
            {
                TriggerRandomFight();
            }
        }
    }

    private void TriggerRandomFight()
    {
        var bossFight = RandomBossFight();
        bossFight.Combat();
        deskManager.FightCompleted(bossFight);
    }

    private BossFight RandomBossFight()
    {
        var player = RandomSpec();
        var enemy = RandomSpec();
        var bossFight = new BossFight(gameDataService, player, enemy);
        return bossFight;
    }

    private SummoningSpecs RandomSpec()
    {
        return new SummoningSpecs(RandomSpecIndex(), RandomSpecIndex(), RandomSpecIndex());
    }

    private int RandomSpecIndex()
    {
        return rand.RandiRange(0, 4);
    }

    private void DeskManager_OnFightCompleted(BossFight bossFigth)
    {
        UpdateLogBook(bossFigth);
    }

    private void UpdateLogBook(BossFight bossFigth)
    {
        lastBossFights.Push(bossFigth);
        UpdateLogBookText();
        CheckIfGiveNewHint(bossFigth);
    }

    private void UpdateLogBookText()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var bossFight in lastBossFights)
        {
            var playerName = gameDataService.GetMonsterName(bossFight.Player);
            var enemyName = gameDataService.GetMonsterName(bossFight.Enemy);
            sb.AppendLine($"{playerName} vs {enemyName}");
            sb.AppendLine($"{bossFight.Result}");
            sb.AppendLine($"{bossFight.Hint}");
            sb.AppendLine();
        }

        logbookTextLabel.Text = sb.ToString();
    }

    private void CheckIfGiveNewHint(BossFight bossFigth)
    {
        if (bossFigth.Result == BossFight.BossFightResult.PlayerWin)
        {
            AddNewVisualHint(bossFigth);
        }
    }

    private void AddNewVisualHint(BossFight bossFigth)
    {
        foreach (var playerWinDetail in bossFigth.PlayerWinsDetails)
        {
            if (IsNewWinDetail(playerWinDetail))
            {
                AddNewWinDetail(playerWinDetail);
                return;
            }
        }
    }

    private bool IsNewWinDetail(BossFightWinDetails playerWinDetail)
    {
        return !WinsDetails.Any(w => w.Equals(playerWinDetail));
    }

    private void AddNewWinDetail(BossFightWinDetails playerWinDetail)
    {
        var playerFrom = gameDataService.GetSpecName(playerWinDetail.Spec, playerWinDetail.PlayerIndex);
        var bossTo = gameDataService.GetSpecName(playerWinDetail.Spec, playerWinDetail.BossIndex);
        GD.Print($"For spec {playerWinDetail.Spec}, show arrow from: {playerFrom} to: {bossTo}");
        GD.Print("arrowNodes count = " + arrowAndSpec2DNodes.Count());

        DisplayNode2D(playerWinDetail.Spec.ToString(), playerFrom);
        DisplayNode2D(playerWinDetail.Spec.ToString(), bossTo);
        DisplayNode2D("Arrow", $"{playerFrom}{bossTo}");

        WinsDetails.Add(playerWinDetail);
    }

    private void DisplayNode2D(string startWith, string searchFor)
    {
        var findArrowNode2D = arrowAndSpec2DNodes.FirstOrDefault(n => n.Name.ToString().StartsWith(startWith) && n.Name.ToString().Contains(searchFor));

        if (findArrowNode2D != null)
        {
            findArrowNode2D.Visible = true;
            GD.Print("Search for key: " + searchFor);
        }
        else
        {
            GD.Print("Search for key: " + searchFor + " (NOT FOUND)");
        }
    }
}
