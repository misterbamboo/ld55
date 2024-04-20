using System;
using System.Collections.Generic;

public class SummoningSpecs
{
    public RotatingValue Emotion { get; private set; }
    public RotatingValue Element { get; private set; }
    public RotatingValue Species { get; private set; }

    public SummoningSpecs()
    {
    }

    public SummoningSpecs(double emotion, double element, double species)
    {
        Emotion = new RotatingValue(emotion,5);
        Element = new RotatingValue(element,5);
        Species = new RotatingValue(species,5);
    }

    public SummoningSpecs(RotatingValue emotion, RotatingValue element, RotatingValue species)
    {
        Emotion = emotion;
        Element = element;
        Species = species;
    }

    public void AddEmotion(double value)
    {
        Emotion += value;
    }

    public void AddElement(double value)
    {
        Element += value;
    }

    public void AddSpecies(double value)
    {
        Species += value;
    }

    public SummoningSpecs Combine(SummoningSpecs other)
    {
        var emotion = Emotion + other.Emotion;
        var element = Element + other.Element;
        var species = Species + other.Species;
        return new SummoningSpecs(emotion, element, species);
    }

    public static SummoningSpecs operator +(SummoningSpecs a, SummoningSpecs b)
    {
        return a.Combine(b);
    }

    public override string ToString()
    {
        return $"(emotion:{Emotion}, species:{Species}, element:{Element})";
    }

    internal SummoningSpecs Clone()
    {
        return new SummoningSpecs(Emotion.Value, Element.Value, Species.Value);
    }

    public static SummoningSpecs Empty()
    {
        return new SummoningSpecs();
    }
}
