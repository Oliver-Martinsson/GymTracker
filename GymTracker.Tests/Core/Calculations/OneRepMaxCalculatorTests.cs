using GymTracker.Core.Calculations;

namespace GymTracker.Tests.Core.Calculations;

public class OneRepMaxCalculatorTests
{
    [Fact]
    public void Epley_Should_ReturnWeight_When_RepsIsOne()
    {
        // Arrange
        double weight = 100;
        int reps = 1;

        // Act
        double actual = OneRepMaxCalculator.Epley(weight, reps);

        // Assert
        Assert.Equal(weight, actual);
    }
        
    [Theory]
    [InlineData(100, 10, 133.33)]
    [InlineData(60, 5, 70)]
    [InlineData(0, 5, 0)]
    [InlineData(100, 0, 0)]
    public void Epley_Should_ReturnEstimatedOneRepMax_When_GivenWeightAndReps(double weight, int reps, double expected)
    {
        // Act
        double actual = OneRepMaxCalculator.Epley(weight, reps);

        // Assert
        Assert.Equal(expected, actual, precision: 1);
    }
}