using System.Linq;

public class BossFight
{
    public BossFightResult Result { get; private set; }

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
    }

    private bool PlayerWin(SummoningSpecs summonSpecs, SummoningSpecs bossSpecs)
    {
        var emotionIndexDistance = summonSpecs.Emotion.IndexDistanceOf(bossSpecs.Emotion);
        var elementIndexDistance = summonSpecs.Element.IndexDistanceOf(bossSpecs.Element);
        var speciesIndexDistance = summonSpecs.Species.IndexDistanceOf(bossSpecs.Species);

        var nbrWin = new[] { emotionIndexDistance, elementIndexDistance, speciesIndexDistance }.Where(i => i > 0).Count();
        var nbrLose = new[] { emotionIndexDistance, elementIndexDistance, speciesIndexDistance }.Where(i => i < 0).Count();
        var nbrDraw = new[] { emotionIndexDistance, elementIndexDistance, speciesIndexDistance }.Where(i => i == 0).Count();

        return nbrWin > nbrLose;
    }

    private bool PlayerLose(SummoningSpecs summonSpecs, SummoningSpecs bossSpecs)
    {
        var emotionIndexDistance = summonSpecs.Emotion.IndexDistanceOf(bossSpecs.Emotion);
        var elementIndexDistance = summonSpecs.Element.IndexDistanceOf(bossSpecs.Element);
        var speciesIndexDistance = summonSpecs.Species.IndexDistanceOf(bossSpecs.Species);

        var nbrWin = new[] { emotionIndexDistance, elementIndexDistance, speciesIndexDistance }.Where(i => i > 0).Count();
        var nbrLose = new[] { emotionIndexDistance, elementIndexDistance, speciesIndexDistance }.Where(i => i < 0).Count();
        var nbrDraw = new[] { emotionIndexDistance, elementIndexDistance, speciesIndexDistance }.Where(i => i == 0).Count();

        return nbrWin < nbrLose;
    }

    public enum BossFightResult
    {
        Undefined,
        Draw,
        PlayerWin,
        PlayerLose
    }
}
