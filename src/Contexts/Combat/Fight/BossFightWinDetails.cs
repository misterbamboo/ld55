using System;

public class BossFightWinDetails : IEquatable<BossFightWinDetails>
{
    public BossFightWinDetails(SpecTypes spec, int playerIndex, int bossIndex)
    {
        Spec = spec;
        PlayerIndex = playerIndex;
        BossIndex = bossIndex;
    }

    public SpecTypes Spec { get; }
    public int PlayerIndex { get; }
    public int BossIndex { get; }

    public override bool Equals(object obj)
    {
        if (obj is BossFightWinDetails bfwd)
        {
            return Equals(bfwd);
        }

        return false;
    }

    public bool Equals(BossFightWinDetails other)
    {
        if (Spec != other.Spec) return false;
        if (PlayerIndex != other.PlayerIndex) return false;
        if (BossIndex != other.BossIndex) return false;

        return false;
    }
}
