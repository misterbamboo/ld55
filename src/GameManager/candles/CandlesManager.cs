using Godot;
using System.Linq;

public partial class CandlesManager : Node
{
    private DeskManager deskManager;

    private bool[] candles = new bool[5];
    private bool[] candlesHover = new bool[5];
    [Export] private Godot.Collections.Array<TextureRect> candleTextures;
    [Export] private Texture2D litCandle;
    [Export] private Texture2D litCandleHover;
    [Export] private Texture2D unlitCandle;
    [Export] private Texture2D unlitCandleHover;

    public override void _Ready()
    {
        deskManager = GetNode<DeskManager>(DeskManager.Path);
        DrawCandles();
    }

    public void ChangeHoverCandle(int index, bool hover)
    {
        candlesHover[index] = hover;
        DrawCandles();
    }

    public void ToggleCandle(int index)
    {
        if(!deskManager.MonsterIsReadyToSummon)
        {
            return;
        }

        candles[index] = !candles[index];
        DrawCandles();

        if (candles.All(x => x))
        {
            deskManager.SummonMonster();
            candles = new bool[5];
            DrawCandles();
        }

        deskManager.CandleToggled(candles[index]);
    }

    public void DrawCandles()
    {
        for (var i = 0; i < candleTextures.Count; i++)
        {
            candleTextures[i].Texture = candles[i] ? GetLitCandle(i) : GetUnlitCandle(i);
        }
    }

    private Texture2D GetLitCandle(int index)
    {
        if (candlesHover[index])
        {
            return litCandleHover;
        }
        return litCandle;
    }

    private Texture2D GetUnlitCandle(int index)
    {
        if (candlesHover[index])
        {
            return unlitCandleHover;
        }
        return unlitCandle;
    }
}
