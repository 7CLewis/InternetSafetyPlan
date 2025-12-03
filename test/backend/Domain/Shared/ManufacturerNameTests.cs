namespace InternetSafetyPlan.Domain.Test.Shared;

public class ManufacturerNameTests
{
    [Fact]
    public void Create_SuccessfullySetsValue()
    {
        // Arrange
        var value = new string('c', ManufacturerName.MaxLength);

        // Act
        var result = ManufacturerName.Create(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsLongerThanMaxLength()
    {
        // Arrange
        var value = new string('c', ManufacturerName.MaxLength + 1);

        // Act
        var result = ManufacturerName.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsEmpty()
    {
        // Arrange
        var value = "";

        // Act
        var result = ManufacturerName.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsWhitespaceOnly()
    {
        // Arrange
        var value = "        ";

        // Act
        var result = ManufacturerName.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }
}
