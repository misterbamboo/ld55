using Xunit;

namespace UnitTests.Shared;

public class SummoningSpecsTests
{
    [Fact]
    public void Create_specs()
    {
        // Arrange
        var specs = new SummoningSpecs();

        // Act

        // Assert
        Assert.True(specs != null);
    }

    [Fact]
    public void Init_specs()
    {
        // Arrange
        var specs = new SummoningSpecs(1.2, 2.3, 3.4);

        // Act

        // Assert
        Assert.Equal(1.2, specs.Emotion.Value);
        Assert.Equal(2.3, specs.Element.Value);
        Assert.Equal(3.4, specs.Species.Value);
    }

    [Fact]
    public void Specs_add_emotion()
    {
        // Arrange
        var specs = new SummoningSpecs();

        // Act
        specs.AddEmotion(1.5);

        // Assert
        Assert.Equal(1.5, specs.Emotion.Value);
    }

    [Fact]
    public void Specs_add_element()
    {
        // Arrange
        var specs = new SummoningSpecs();

        // Act
        specs.AddElement(1.5);

        // Assert
        Assert.Equal(1.5, specs.Element.Value);
    }

    [Fact]
    public void Specs_add_species()
    {
        // Arrange
        var specs = new SummoningSpecs();

        // Act
        specs.AddSpecies(1.5);

        // Assert
        Assert.Equal(1.5, specs.Species.Value);
    }

    [Fact]
    public void Combine_specs()
    {
        // Arrange
        var specs1 = new SummoningSpecs(1, 3, 4.2);
        var specs2 = new SummoningSpecs(2, 3, 4.4);

        // Act
        var result = specs1.Combine(specs2);

        // Assert
        Assert.Equal(3, result.Emotion);
        Assert.Equal(1f, result.Element);
        Assert.Equal(3.6d, result.Species);
    }

    [Fact]
    public void Combine_specs_with_plus_sign()
    {
        // Arrange
        var specs1 = new SummoningSpecs(1, 3, 4.2);
        var specs2 = new SummoningSpecs(2, 3, 4.4);

        // Act
        var result = specs1 + specs2;

        // Assert
        Assert.Equal(3, result.Emotion);
        Assert.Equal(1f, result.Element);
        Assert.Equal(3.6d, result.Species);
    }
}