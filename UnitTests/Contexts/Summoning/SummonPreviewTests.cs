using Xunit;

namespace UnitTests;

public class SummonPreviewTests
{

    [Fact]
    public void Adding_value()
    {
        // Arrange
        var value = new SummoningValue();

        // Act
        value.Add(4.5);

        // Assert
        Assert.Equal(4.5, value.Value);
    }

    [Fact]
    public void Init_value()
    {
        // Arrange
        var value = new SummoningValue(2);

        // Act

        // Assert
        Assert.Equal(2, value.Value);
    }

    [Fact]
    public void Adding_multiple_values_sum_up()
    {
        // Arrange
        var value = new SummoningValue();

        // Act
        value.Add(4.5);

        // Assert
        Assert.Equal(4.5, value.Value);
    }
}