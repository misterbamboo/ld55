using Godot;
using System;

public partial class DeskManager : Node
{
	public const string Path = "/root/DeskManager";

    public delegate void GameStartedHandler();
    public delegate void GameStopedHandler();

    public delegate void ArcaneFocusFilledHandler(Ingredient ingredient);
    public delegate void ArcaneFocusAdjustedHandler(Ingredient ingredient);
    public delegate void ArcaneFocusEmptiedHandler();

    public delegate void MonsterStatsUpdatedHandler(SummoningSpecs monster);
    public delegate void MonsterSummonedHandler(SummoningSpecs monster);
    public delegate void FightHandler(BossFight bossFigth);

    public event MonsterStatsUpdatedHandler OnMonsterStatsUpdated;
    public event MonsterSummonedHandler OnMonsterSummoned;
    public event FightHandler OnFight;

	public event GameStartedHandler OnGameStart;
    public event GameStopedHandler OnGameStop;

	public event ArcaneFocusFilledHandler OnArcaneFocusFilled;
    public event ArcaneFocusAdjustedHandler OnArcaneFocusAdjusted;
	public event ArcaneFocusEmptiedHandler OnArcaneFocusEmptied;

	private bool GameStarted = false;

    private SummoningSpecs currentMonsterStats = new SummoningSpecs(2.5,2.5,2.5);
    public SummoningSpecs CurrentMonsterStats => currentMonsterStats;
    
    public override void _Process(double delta)
    {
        if(Input.IsActionPressed("ui_cancel"))
        {
            GetTree().Quit();
        }
    }

    public void StartGame()
    {
        GameStarted = true;
        GD.PrintRich("[color=cyan]DeskEvent: OnGameStart[/color]");
        OnGameStart?.Invoke();
    }

    public void StopGame()
    {
        GameStarted = false;
        GD.PrintRich("[color=cyan]DeskEvent: OnGameStop[/color]");
        OnGameStop?.Invoke();
    }

	public void SummonMonster()
	{
        GD.PrintRich("[color=cyan]DeskEvent: OnMonsterSummoned[/color]");
		OnMonsterSummoned?.Invoke(currentMonsterStats);
	}

    public void NewMonster(SummoningSpecs monster)
    {
        currentMonsterStats = monster;
        GD.PrintRich("[color=cyan]DeskEvent: OnMonsterStatsUpdated[/color]");
        OnMonsterStatsUpdated?.Invoke(currentMonsterStats);
    }

    public void UpdateMonsterStats(SummoningSpecs monster)
    {
        currentMonsterStats = monster;
        GD.PrintRich("[color=cyan]DeskEvent: OnMonsterStatsUpdated[/color]");
        OnMonsterStatsUpdated?.Invoke(currentMonsterStats);
    }

    public void FillArcaneForcus(Ingredient ingredient)
    {
        GD.PrintRich("[color=cyan]DeskEvent: OnArcaneFocusFilled[/color]");
        OnArcaneFocusFilled?.Invoke(ingredient);
    }

	public void EmptyArcaneForcus()
	{
        GD.PrintRich("[color=cyan]DeskEvent: OnArcaneFocusEmptied[/color]");
        OnArcaneFocusEmptied?.Invoke();
    }

    public void AdjustArcaneFocus(Ingredient ingredient)
    {
        //GD.PrintRich("[color=cyan]DeskEvent: OnArcaneFocusAdjusted[/color]");
        OnArcaneFocusAdjusted?.Invoke(ingredient);
    }

    public void CompleteFight(BossFight bossFight)
    {
        GD.PrintRich("[color=cyan]DeskEvent: OnFightCompleted[/color]");
        OnFight?.Invoke(bossFight);
    }

    public void SceneLoaded()
    {
        StartGame();
    }
}
