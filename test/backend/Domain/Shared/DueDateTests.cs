namespace InternetSafetyPlan.Domain.Test.Shared;

public class DueDateTests
{
    [Fact]
    public void Create_SuccessfullySetsValue()
    {
        // Arrange
        var value = new DateTime(2189, 12, 13);

        // Act
        var result = DueDate.Create(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsInPast()
    {
        // Arrange
        var value = new DateTime(1989, 12, 13);

        // Act
        var result = DueDate.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }
}
