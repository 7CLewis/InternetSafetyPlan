namespace InternetSafetyPlan.Domain.Test.Shared;

public class EmailTests
{
    [Fact]
    public void Create_SuccessfullySetsValue()
    {
        // Arrange
        var value = new string('c', Email.MaxLength);

        // Act
        var result = Email.Create(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsLongerThanMaxLength()
    {
        // Arrange
        var value = new string('c', Email.MaxLength + 1);

        // Act
        var result = Email.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsEmpty()
    {
        // Arrange
        var value = "";

        // Act
        var result = Email.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsWhitespaceOnly()
    {
        // Arrange
        var value = "        ";

        // Act
        var result = Email.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }
}
