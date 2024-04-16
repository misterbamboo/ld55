public class Ease
{
    public static double Spike(double t)
    {
        if (t <= .5f)
            return EaseIn(t / .5f);

        return EaseIn(Flip(t) / .5f);
    }

    public static double Flip(double t)
    {
        return 1 - t;
    }

    public static double EaseIn(double t)
    {
        return t * t;
    }
}

