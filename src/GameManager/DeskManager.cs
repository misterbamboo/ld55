using Godot;
using System;
using System.Collections.Generic;

public partial class DeskManager : Node
{
    public const string Path = "/root/DeskManager";

    public delegate void GameStartedHandler();
    public delegate void GameStopedHandler();

    public delegate void IngredientOnSummoningBoardUpdatedHandler(IEnumerable<Ingredient> ingregients);
    public delegate void ArcaneFocusFilledHandler(Ingredient ingredient);
    public delegate void ArcaneFocusAdjustedHandler(Ingredient ingredient);
    public delegate void ArcaneFocusEmptiedHandler();

    public delegate void MonsterSummonedHandler(SummoningSpecs monster);
    public delegate void MonsterReadyToSummonHandler(SummoningSpecs monster);
    public delegate void FightHandler(BossFight bossFigth);

    public event IngredientOnSummoningBoardUpdatedHandler OnIngredientsOnSummoningBoardUpdated;
    public event MonsterSummonedHandler OnMonsterSummoned;
    public event MonsterReadyToSummonHandler OnMonsterReadyToSummon;

    public event GameStartedHandler OnGameStart;
    public event GameStopedHandler OnGameStop;

    public event ArcaneFocusFilledHandler OnArcaneFocusFilled;
    public event ArcaneFocusAdjustedHandler OnArcaneFocusAdjusted;
    public event ArcaneFocusEmptiedHandler OnArcaneFocusEmptied;

    public bool GameStarted { get; private set; } = false;

    private SummoningSpecs currentMonsterStats;
    public SummoningSpecs CurrentMonsterStats => currentMonsterStats;

    public bool MonsterIsReadyToSummon => currentMonsterStats != null;

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ui_cancel"))
        {
            GetTree().Quit();
        }
    }

    public void StartGame()
    {
        currentMonsterStats = null;
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
        currentMonsterStats = null;
    }

    public void UpdateIngredientsOnSummoningBoard(IEnumerable<Ingredient> ingregients)
    {
        GD.PrintRich("[color=cyan]DeskEvent: OnMonsterStatsUpdated[/color]");
        OnIngredientsOnSummoningBoardUpdated?.Invoke(ingregients);
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

    public void ReadyMonsterForSummon(SummoningSpecs monster)
    {
        currentMonsterStats = monster;
        GD.PrintRich("[color=cyan]DeskEvent: OnMonsterReadyToSummon[/color]");
        GD.PrintRich($"[color=cyan]{monster}[/color]");
        OnMonsterReadyToSummon?.Invoke(monster);
    }

    public event FightHandler OnFightCompleted;
    public void FightCompleted(BossFight bossFight)
    {
        GD.PrintRich($"[color=cyan]DeskEvent: {nameof(OnFightCompleted)}[/color]");
        OnFightCompleted?.Invoke(bossFight);
    }

    public void SceneLoaded()
    {
        StartGame();
    }

    public event Action OnArcaneFocusDragStart;
    internal void ArcaneFocusDragStart()
    {
        GD.PrintRich("[color=cyan]DeskEvent: OnArcaneFocusDragStart[/color]");
        OnArcaneFocusDragStart?.Invoke();
    }

    public event Action OnArcaneFocusDragEnd;
    internal void ArcaneFocusDragEnd()
    {
        GD.PrintRich($"[color=cyan]DeskEvent: {nameof(OnArcaneFocusDragEnd)}[/color]");
        OnArcaneFocusDragEnd?.Invoke();
    }

    public event Action<bool> OnCandleToggled;
    internal void CandleToggled(bool state)
    {
        GD.PrintRich($"[color=cyan]DeskEvent: {nameof(OnCandleToggled)}[/color]");
        OnCandleToggled?.Invoke(state);
    }

    public event Action<SummoningSpecs, SummoningSpecs> OnFightStarting;
    internal void FightStarting(SummoningSpecs playerSummoningSpecs, SummoningSpecs bossSummoningSpecs)
    {
        GD.PrintRich($"[color=cyan]DeskEvent: {nameof(OnFightStarting)}[/color]");
        OnFightStarting?.Invoke(playerSummoningSpecs, bossSummoningSpecs);
    }


    public event Action OnItemGrabbed;
    internal void ItemGrabbed()
    {
        GD.PrintRich($"[color=cyan]DeskEvent: {nameof(OnItemGrabbed)}[/color]");
        OnItemGrabbed?.Invoke();
    }

    public event Action OnItemDropped;
    internal void ItemDropped()
    {
        GD.PrintRich($"[color=cyan]DeskEvent: {nameof(OnItemDropped)}[/color]");
        OnItemDropped?.Invoke();
    }

    public event Action OnCardMoving;
    internal void CardMoving()
    {
        GD.PrintRich($"[color=cyan]DeskEvent: {nameof(OnCardMoving)}[/color]");
        OnCardMoving?.Invoke();
    }

    public event Action OnFightHit;
    internal void FightHit()
    {
        GD.PrintRich($"[color=cyan]DeskEvent: {nameof(OnFightHit)}[/color]");
        OnFightHit?.Invoke();
    }
}
