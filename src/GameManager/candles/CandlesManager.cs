using Godot;
using System;
using System.Linq;

public partial class CandlesManager : Node
{
	private GameDataService gameDataService;
	private bool[] candles = new bool[5];
	[Export] private SummoningStatsUI summoningStatsUI;
	[Export] private InventoryService inventoryService;
	[Export] private Godot.Collections.Array<TextureRect> candleTextures;
	[Export] private Texture2D litCandle;
	[Export] private Texture2D unlitCandle;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		inventoryService = GetNode<InventoryService>("../InventoryService");
		summoningStatsUI = GetNode<SummoningStatsUI>("../SummoningStatsUI");
		gameDataService = GetNode<GameDataService>(GameDataService.Path);
		DrawCandles();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ToggleCandle(int index)
	{
		GD.Print($"Toggle {index}");
		candles[index] = !candles[index];
		DrawCandles();

		if(candles.All(x => x))
		{
			var bossfight = new BossFight(gameDataService);
			var bossSpecs = new SummoningSpecs(2, 2, 2);
			bossfight.Combat(summoningStatsUI.monsterSpecs, bossSpecs);

			GD.Print(bossfight.Result);
			GD.Print(bossfight.Hint);
			candles = new bool[5];
			inventoryService.ClearSummoningBoard();

            DrawCandles();
        }
	}

	public void DrawCandles()
	{
		for (var i = 0; i < candleTextures.Count;  i++)
		{
			candleTextures[i].Texture = candles[i] ? litCandle : unlitCandle;
		}
	}
}
