using System;
using System.Collections.Generic;
using System.Linq;

public class BossFight
{
    public SummoningSpecs Player { get; }
    public SummoningSpecs Enemy { get; }
    public BossFightResult Result { get; private set; }
    public string Hint { get; private set; }

    public int PlayerWins = 0;
    public int PlayerLoses = 0;
    public int PlayerDraws = 0;


    private List<BossFightWinDetails> _playerWinsDetails = new List<BossFightWinDetails>();
    public IEnumerable<BossFightWinDetails> PlayerWinsDetails => _playerWinsDetails;

    private IHintProvider HintProvider { get; }

    public BossFight(IHintProvider hintProvider, SummoningSpecs player, SummoningSpecs enemy)
    {
        HintProvider = hintProvider;
        Player = player;
        Enemy = enemy;
    }

    public void Combat()
    {
        ComputeCombat(Player, Enemy);

        if (PlayerWin(Player, Enemy))
        {
            Result = BossFightResult.PlayerWin;
        }
        else if (PlayerLose(Player, Enemy))
        {
            Result = BossFightResult.PlayerLose;
        }
        else
        {
            Result = BossFightResult.Draw;
        }
        ChooseHint(Player, Enemy);
    }

    private void ComputeCombat(SummoningSpecs summonSpecs, SummoningSpecs bossSpecs)
    {
        var emotionIndexDistance = summonSpecs.Emotion.InvertedIndexDistanceOf(bossSpecs.Emotion);
        var elementIndexDistance = summonSpecs.Element.InvertedIndexDistanceOf(bossSpecs.Element);
        var speciesIndexDistance = summonSpecs.Species.InvertedIndexDistanceOf(bossSpecs.Species);

        ComputeWinDrawLoseCount(emotionIndexDistance, elementIndexDistance, speciesIndexDistance);

        ComputeWinDetails(
            emotionIndexDistance,
            elementIndexDistance,
            speciesIndexDistance,
            summonSpecs,
            bossSpecs);
    }

    private void ComputeWinDrawLoseCount(int emotionIndexDistance, int elementIndexDistance, int speciesIndexDistance)
    {
        var indexDistances = new[] { emotionIndexDistance, elementIndexDistance, speciesIndexDistance };
        PlayerWins = indexDistances.Where(i => IsWinning(i)).Count();
        PlayerLoses = indexDistances.Where(i => IsLosing(i)).Count();
        PlayerDraws = indexDistances.Where(i => IsDraw(i)).Count();
    }

    private static bool IsWinning(int i) => i > 0;
    private static bool IsLosing(int i) => i < 0;
    private static bool IsDraw(int i) => i == 0;

    private void ComputeWinDetails(
        int emotionIndexDistance,
        int elementIndexDistance,
        int speciesIndexDistance,
        SummoningSpecs summonSpecs,
        SummoningSpecs bossSpecs)
    {
        if (IsWinning(emotionIndexDistance))
        {
            AppendWinDetailsFor(SpecTypes.Emotion, summonSpecs.Emotion.Index, bossSpecs.Emotion.Index);
        }

        if (IsWinning(elementIndexDistance))
        {
            AppendWinDetailsFor(SpecTypes.Element, summonSpecs.Element.Index, bossSpecs.Element.Index);
        }

        if (IsWinning(speciesIndexDistance))
        {
            AppendWinDetailsFor(SpecTypes.Species, summonSpecs.Species.Index, bossSpecs.Species.Index);
        }
    }

    private void AppendWinDetailsFor(SpecTypes spec, int playerIndex, int bossIndex)
    {
        _playerWinsDetails.Add(new BossFightWinDetails(spec, playerIndex, bossIndex));
    }

    private bool PlayerWin(SummoningSpecs summonSpecs, SummoningSpecs bossSpecs)
    {
        return PlayerWins > PlayerLoses;
    }

    private bool PlayerLose(SummoningSpecs summonSpecs, SummoningSpecs bossSpecs)
    {
        return PlayerWins < PlayerLoses;
    }

    /// <summary>
    /// TODO: DOUBLE CHECK THIS WITH GD.PRINT()
    /// </summary>
    private void ChooseHint(SummoningSpecs summonSpecs, SummoningSpecs bossSpecs)
    {
        var fightResults = new List<BossFightResultSpecInfo>();
        FillFightResults(fightResults, SpecTypes.Emotion, summonSpecs.Emotion, bossSpecs.Emotion);
        FillFightResults(fightResults, SpecTypes.Element, summonSpecs.Element, bossSpecs.Element);
        FillFightResults(fightResults, SpecTypes.Species, summonSpecs.Species, bossSpecs.Species);

        var hintSpecType = fightResults.Where(f => f.BossFightResult == Result).FirstOrDefault();

        Hint = HintProvider.GetHintFor(hintSpecType.SpecType, hintSpecType.SummonIndex, hintSpecType.BossIndex);
    }

    /// <summary>
    /// TODO: DOUBLE CHECK THIS WITH GD.PRINT()
    /// </summary>
    private void FillFightResults(
        List<BossFightResultSpecInfo> fightResults,
        SpecTypes specType,
        RotatingValue summonValue,
        RotatingValue bossValue)
    {
        var invertedIndexDistance = summonValue.InvertedIndexDistanceOf(bossValue);
        var result = GetBossFightResultOfDistance(invertedIndexDistance);
        fightResults.Add(new BossFightResultSpecInfo(result, summonValue.Index, bossValue.Index, specType));
    }

    private BossFightResult GetBossFightResultOfDistance(int invertedIndexDistance)
    {
        return
            invertedIndexDistance < 0 ? BossFightResult.PlayerLose :
            invertedIndexDistance > 0 ? BossFightResult.PlayerWin :
            BossFightResult.Draw;
    }

    public enum BossFightResult
    {
        Undefined,
        Draw,
        PlayerWin,
        PlayerLose
    }
}
