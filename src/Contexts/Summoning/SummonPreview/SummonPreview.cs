
using System;

public class SummonPreview
{
    public SummoningValue Emotion { get; set; }
    public SummoningValue Element { get; set; }
    public SummoningValue Species { get; set; }

    public void Do()
    {
        Emotion.Add(0.25);
    }
}

public class SummoningValue
{
    private const double MaxCount = 5;
    public double Value { get; private set; }
    public int Index => (int)Value;

    public SummoningValue()
    {
    }

    public SummoningValue(double init)
    {
        Value = init;
    }

    public void Add(double added)
    {
        var sum = Value + added;
        Value = sum % MaxCount;
    }
}