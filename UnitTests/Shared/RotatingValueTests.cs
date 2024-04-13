using Xunit;

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
    public void Sum_2_RotatingValues_sum_up()
    {
        // Arrange
        var value1 = new RotatingValue(2);
        var value2 = new RotatingValue(2);

        // Act
        var result = value1 + value2;

        // Assert
        Assert.Equal(4, result.Value);
    }

    [Fact]
    public void Sum_2_RotatingValues_should_stay_in_range_of_0_5()
    {
        // Arrange
        var value1 = new RotatingValue(3);
        var value2 = new RotatingValue(3);

        // Act
        var result = value1 + value2;

        // Assert
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Sum_2_negative_RotatingValues_should_stay_in_range_of_0_5()
    {
        // Arrange
        var value1 = new RotatingValue(-1);
        var value2 = new RotatingValue(-2);

        // Act
        var result = value1 + value2;

        // Assert
        Assert.Equal(2, result.Value);
    }

    [Fact]
    public void Subtract_2_RotatingValues_should_stay_in_range_of_0_5()
    {
        // Arrange
        var value1 = new RotatingValue(2);
        var value2 = new RotatingValue(4);

        // Act
        var result = value1 - value2;

        // Assert
        Assert.Equal(3, result.Value);
    }

    [Fact]
    public void Multiply_2_RotatingValues_should_stay_in_range_of_0_5()
    {
        // Arrange
        var value1 = new RotatingValue(2);
        var value2 = new RotatingValue(3);

        // Act
        var result = value1 * value2;

        // Assert
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Equality_with_2_RotatingValues_should_work()
    {
        // Arrange
        var value1 = new RotatingValue(2);
        var value2 = new RotatingValue(2);

        // Act

        // Assert
        Assert.True(value1 == value2);
    }

    [Fact]
    public void NOT_Equality_with_2_RotatingValues_should_work()
    {
        // Arrange
        var value1 = new RotatingValue(2);
        var value2 = new RotatingValue(3);

        // Act

        // Assert
        Assert.True(value1 != value2);
    }

    [Fact]
    public void Add_value_to_SummoningValue_should_not_change_original_value()
    {
        // Arrange
        var value = new RotatingValue(2);

        // Act
        value.Add(3);

        // Assert
        Assert.Equal(2, value.Value);
    }

    [Fact]
    public void Add_or_subtract_int_to_SummoningValue()
    {
        // Arrange
        var value = new RotatingValue(1);

        // Act
        var addResult = value + 3;
        var subResult = value - 2;
        var addResult2 = 3 + value;
        var subResult2 = 2 - value;

        // Assert
        Assert.Equal(4, addResult.Value);
        Assert.Equal(4, subResult.Value);
        Assert.Equal(4, addResult2.Value);
        Assert.Equal(4, subResult2.Value);
    }

    [Fact]
    public void Multiply_int_to_SummoningValue()
    {
        // Arrange
        var value = new RotatingValue(2);

        // Act
        var timesResult = value * 3;
        var timesResult2 = 2 * value;

        // Assert
        Assert.Equal(1, timesResult.Value);
        Assert.Equal(4, timesResult2.Value);
    }

    [Fact]
    public void Equality_with_int()
    {
        // Arrange
        var value = new RotatingValue(3);

        // Act

        // Assert
        Assert.True(3 == value);
        Assert.True(2 != value);
        Assert.True(value == 3);
        Assert.True(value != 2);
    }

    [Fact]
    public void Add_or_subtract_double_to_SummoningValue()
    {
        // Arrange
        var value = new RotatingValue(1);

        // Act
        var addResult = value + 3d;
        var subResult = value - 2d;
        var addResult2 = 3d + value;
        var subResult2 = 2d - value;

        // Assert
        Assert.Equal(4d, addResult.Value);
        Assert.Equal(4d, subResult.Value);
        Assert.Equal(4d, addResult2.Value);
        Assert.Equal(4d, subResult2.Value);
    }

    [Fact]
    public void Multiply_double_to_SummoningValue()
    {
        // Arrange
        var value = new RotatingValue(2);

        // Act
        var timesResult = value * 3d;
        var timesResult2 = 2d * value;

        // Assert
        Assert.Equal(1d, timesResult.Value);
        Assert.Equal(4d, timesResult2.Value);
    }

    [Fact]
    public void Equality_with_double()
    {
        // Arrange
        var value = new RotatingValue(3);

        // Act

        // Assert
        Assert.True(3d == value);
        Assert.True(2d != value);
        Assert.True(value == 3d);
        Assert.True(value != 2d);
    }

    [Fact]
    public void Add_or_subtract_float_to_SummoningValue()
    {
        // Arrange
        var value = new RotatingValue(1);

        // Act
        var addResult = value + 3f;
        var subResult = value - 2f;
        var addResult2 = 3f + value;
        var subResult2 = 2f - value;

        // Assert
        Assert.Equal(4f, addResult.Value);
        Assert.Equal(4f, subResult.Value);
        Assert.Equal(4f, addResult2.Value);
        Assert.Equal(4f, subResult2.Value);
    }

    [Fact]
    public void Multiply_float_to_SummoningValue()
    {
        // Arrange
        var value = new RotatingValue(2);

        // Act
        var timesResult = value * 3f;
        var timesResult2 = 2f * value;

        // Assert
        Assert.Equal(1f, timesResult.Value);
        Assert.Equal(4f, timesResult2.Value);
    }

    [Fact]
    public void Equality_with_float()
    {
        // Arrange
        var value = new RotatingValue(3);

        // Act

        // Assert
        Assert.True(3f == value);
        Assert.True(2f != value);
        Assert.True(value == 3f);
        Assert.True(value != 2f);
    }

    [Fact]
    public void Equals_method_int_float_double()
    {
        // Arrange
        var value = new RotatingValue(3);
        
        // Act

        // Assert
        Assert.True(value.Equals(3));
        Assert.True(value.Equals(3f));
        Assert.True(value.Equals(3d));
    }
}