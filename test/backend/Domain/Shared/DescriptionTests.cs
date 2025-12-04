namespace InternetSafetyPlan.Domain.Test.Shared;

public class DescriptionTests
{
    [Fact]
    public void Create_SuccessfullySetsValue()
    {
        // Description
        var value = new string('c', Description.MaxLength);

        // Act
        var result = Description.Create(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsLongerThanMaxLength()
    {
        // Arrange
        var value = new string('c', Description.MaxLength + 1);

        // Act
        var result = Description.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }
}
