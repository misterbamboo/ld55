using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public partial class LogbookSceneUI : Node
{
    [Export] private RichTextLabel logbookTextLabel;
    private GameDataService gameDataService;
    private DeskManager deskManager;
    private RandomNumberGenerator rand;
    private Stack<BossFight> lastBossFights = new Stack<BossFight>();

    public override void _Ready()
    {
        gameDataService = GetNode<GameDataService>(GameDataService.Path);
        deskManager = GetNode<DeskManager>(DeskManager.Path);
        rand = new RandomNumberGenerator();
        rand.Randomize();
        deskManager.OnFightCompleted += DeskManager_OnFightCompleted;
    }

    bool lastState = false;
    public override void _Process(double delta)
    {
        var newState = Input.IsKeyPressed(Key.R);
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
        deskManager.FightCompleted(RandomBossFight());
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
        lastBossFights.Push(bossFigth);
        UpdateLogBook();
    }

    private void UpdateLogBook()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var lastBossFight in lastBossFights)
        {
            var playerName = gameDataService.GetMonsterName(lastBossFight.Player);
            var enemyName = gameDataService.GetMonsterName(lastBossFight.Enemy);
            sb.AppendLine($"{playerName} vs {enemyName}");
        }

        logbookTextLabel.Text = sb.ToString();
    }
}
