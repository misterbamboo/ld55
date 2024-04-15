using System.Collections.Generic;
using System.Linq;

public class BossFight
{
    public BossFightResult Result { get; private set; }
    public string Hint { get; private set; }

    public int PlayerWins = 0;

    private IHintProvider HintProvider { get; }

    public BossFight(IHintProvider hintProvider)
    {
        HintProvider = hintProvider;
    }

    public void Combat(SummoningSpecs summonSpecs, SummoningSpecs bossSpecs)
    {
        if (PlayerWin(summonSpecs, bossSpecs))
        {
            Result = BossFightResult.PlayerWin;
        }
        else if (PlayerLose(summonSpecs, bossSpecs))
        {
            Result = BossFightResult.PlayerLose;
        }
        else
        {
            Result = BossFightResult.Draw;
        }
        ChooseHint(summonSpecs, bossSpecs);
    }

    private bool PlayerWin(SummoningSpecs summonSpecs, SummoningSpecs bossSpecs)
    {
        var emotionIndexDistance = summonSpecs.Emotion.InvertedIndexDistanceOf(bossSpecs.Emotion);
        var elementIndexDistance = summonSpecs.Element.InvertedIndexDistanceOf(bossSpecs.Element);
        var speciesIndexDistance = summonSpecs.Species.InvertedIndexDistanceOf(bossSpecs.Species);

        var indexDistances = new[] { emotionIndexDistance, elementIndexDistance, speciesIndexDistance };
        var nbrWin = indexDistances.Where(i => i > 0).Count();
        var nbrLose = indexDistances.Where(i => i < 0).Count();
        var nbrDraw = indexDistances.Where(i => i == 0).Count();
        PlayerWins = nbrWin;

        return nbrWin > nbrLose;
    }

    private bool PlayerLose(SummoningSpecs summonSpecs, SummoningSpecs bossSpecs)
    {
        var emotionIndexDistance = summonSpecs.Emotion.InvertedIndexDistanceOf(bossSpecs.Emotion);
        var elementIndexDistance = summonSpecs.Element.InvertedIndexDistanceOf(bossSpecs.Element);
        var speciesIndexDistance = summonSpecs.Species.InvertedIndexDistanceOf(bossSpecs.Species);

        var indexDistances = new[] { emotionIndexDistance, elementIndexDistance, speciesIndexDistance };
        var nbrWin = indexDistances.Where(i => i > 0).Count();
        var nbrLose = indexDistances.Where(i => i < 0).Count();
        var nbrDraw = indexDistances.Where(i => i == 0).Count();

        return nbrWin < nbrLose;
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
