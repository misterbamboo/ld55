using Xunit;
using static Godot.HttpRequest;

namespace UnitTests.Shared;

public class RotatingValueTests
{
    [Fact]
    public void Adding_value()
    {
        // Arrange
        var value = new RotatingValue();

        // Act
        var result = value.Add(4.5);

        // Assert
        Assert.Equal(4.5, result.Value);
    }

    [Fact]
    public void Adding_index()
    {
        // Arrange
        var value = new RotatingValue();

        // Act
        var result = value.Add(4.5);

        // Assert
        Assert.Equal(4, result.Index);
    }

    [Fact]
    public void Init_value()
    {
        // Arrange
        var value = new RotatingValue(2);

        // Act

        // Assert
        Assert.Equal(2, value.Value);
    }


    [Fact]
    public void Init_index()
    {
        // Arrange
        var value = new RotatingValue(2.5);

        // Act

        // Assert
        Assert.Equal(2, value.Index);
    }

    [Fact]
    public void Adding_value_to_init_value_sum_up()
    {
        // Arrange
        var value = new RotatingValue(1);

        // Act
        var result = value.Add(3.5);

        // Assert
        Assert.Equal(4.5, result.Value);
    }

    [Fact]
    public void Adding_value_to_init_value_sum_up_index()
    {
        // Arrange
        var value = new RotatingValue(1);

        // Act
        var result = value.Add(3.5);

        // Assert
        Assert.Equal(4, result.Index);
    }

    [Fact]
    public void Adding_multiple_values_sum_up()
    {
        // Arrange
        var value = new RotatingValue();

        // Act
        var resultA = value.Add(1.5);
        var resultB = resultA.Add(1.6);

        // Assert
        Assert.Equal(3.1, resultB.Value);
    }

    [Fact]
    public void Overlapping_5_return_to_0()
    {
        // Arrange
        var value = new RotatingValue();

        // Act
        var result = value.Add(6);

        // Assert
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Overlapping_6dot5_return_to_1_Index()
    {
        // Arrange
        var value = new RotatingValue();

        // Act
        var result = value.Add(6.5);

        // Assert
        Assert.Equal(1, result.Index);
    }

    [Fact]
    public void Negative_values_return_to_range_between_0_5()
    {
        // Arrange
        var value = new RotatingValue();

        // Act
        var result = value.Add(-1);

        // Assert
        Assert.Equal(4, result.Value);
    }

    [Fact]
    public void Negative_0dot5_value_return_index_4()
    {
        // Arrange
        var value = new RotatingValue();

        // Act
        var result = value.Add(-0.5);

        // Assert
        Assert.Equal(4, result.Index);
    }

    [Fact]
    public void BIG_negative_value_return_to_range_between_0_5()
    {
        // Arrange
        var value = new RotatingValue();

        // Act
        var result = value.Add(-97);

        // Assert
        Assert.Equal(3, result.Value);
    }

    [Fact]
    public void BIG_positive_value_return_to_range_between_0_5()
    {
        // Arrange
        var value = new RotatingValue();

        // Act
        var result = value.Add(97);

        // Assert
        Assert.Equal(2, result.Value);
    }

    [Fact]
    public void Closest_index_distance_between_2_rotatingValues_without_overflow()
    {
        // Arrange
        var value1 = new RotatingValue(1);
        var value2 = new RotatingValue(2);

        // Act
        var result1to2 = value1.IndexDistanceOf(value2);
        var result2to1 = value2.IndexDistanceOf(value1);

        // Assert
        Assert.Equal(1, result1to2);
        Assert.Equal(-1, result2to1);
    }

    [Fact]
    public void Closest_index_distance_between_2_rotatingValues_with_overflow_fromLeft()
    {
        // Arrange
        var valueA = new RotatingValue(1);
        var valueB = new RotatingValue(4);

        // Act
        var resultFromLeft = valueA.IndexDistanceOf(valueB);

        // Assert
        Assert.Equal(-2, resultFromLeft);
    }

    [Fact]
    public void Closest_index_distance_between_2_rotatingValues_with_overflow_fromRight()
    {
        // Arrange
        var value4 = new RotatingValue(4);
        var value1 = new RotatingValue(1);

        // Act
        var resultFromRight = value4.IndexDistanceOf(value1);
        var resultFromRight2 = value1.IndexDistanceOf(value4);

        // Assert
        Assert.Equal(2, resultFromRight);
        Assert.Equal(-2, resultFromRight2);
    }

    [Fact]
    public void Closest_distance_between_2_rotatingValues_without_overflow()
    {
        // Arrange
        var valueA = new RotatingValue(0.8);
        var valueB = new RotatingValue(2.2);

        // Act
        var resultAtoB = valueA.DistanceOf(valueB);
        var resultBtoA = valueB.DistanceOf(valueA);

        // Assert
        Assert.Equal(1.4, resultAtoB);
        Assert.Equal(-1.4, resultBtoA);
    }

    [Fact]
    public void Closest_distance_between_2_rotatingValues_with_overflow_fromLeft()
    {
        // Arrange
        var valueA = new RotatingValue(0.6);
        var valueB = new RotatingValue(4.4);

        // Act
        var resultFromLeft = valueA.DistanceOf(valueB);

        // Assert
        Assert.Equal(-1.2, resultFromLeft);
    }

    [Fact]
    public void Closest_distance_between_2_rotatingValues_with_overflow_fromRight()
    {
        // Arrange
        var value4 = new RotatingValue(4.8);
        var value1 = new RotatingValue(0.3);

        // Act
        var resultFromRight = value4.DistanceOf(value1);
        var resultFromRight2 = value1.DistanceOf(value4);

        // Assert
        Assert.Equal(0.5, resultFromRight);
        Assert.Equal(-0.5, resultFromRight2);
    }
}