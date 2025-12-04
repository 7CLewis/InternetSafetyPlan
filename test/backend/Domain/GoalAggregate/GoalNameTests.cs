namespace InternetSafetyPlan.Domain.Test.GoalAggregate;

public class GoalNameTests
{
    [Fact]
    public void Create_SuccessfullySetsValue()
    {
        // Arrange
        var value = new string('c', GoalName.MaxLength);

        // Act
        var result = GoalName.Create(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsLongerThanMaxLength()
    {
        // Arrange
        var value = new string('c', GoalName.MaxLength + 1);

        // Act
        var result = GoalName.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsEmpty()
    {
        // Arrange
        var value = "";

        // Act
        var result = GoalName.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsWhitespaceOnly()
    {
        // Arrange
        var value = "        ";

        // Act
        var result = GoalName.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }
}
