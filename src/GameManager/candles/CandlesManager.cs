using Godot;
using System;
using System.Linq;

public partial class CandlesManager : Node
{
	private DeskManager deskManager;

	private bool[] candles = new bool[5];
	[Export] private Godot.Collections.Array<TextureRect> candleTextures;
	[Export] private Texture2D litCandle;
	[Export] private Texture2D unlitCandle;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		deskManager = GetNode<DeskManager>(DeskManager.Path);
		DrawCandles();
	}

	public void ToggleCandle(int index)
	{
		candles[index] = !candles[index];
		DrawCandles();

		if(candles.All(x => x))
		{
			deskManager.SummonMonster();
			candles = new bool[5];
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
