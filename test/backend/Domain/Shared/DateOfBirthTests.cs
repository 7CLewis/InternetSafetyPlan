namespace InternetSafetyPlan.Domain.Test.Shared;

public class DateOfBirthTests
{
    [Fact]
    public void Create_SuccessfullySetsValue()
    {
        // Arrange
        var value = new DateTime(1989, 12, 13);

        // Act
        var result = DateOfBirth.Create(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Fact]
    public void Create_ReturnsFailureResultWhen_ValueIsBefore1900()
    {
        // Arrange
        var value = new DateTime(1899, 12, 31);

        // Act
        var result = DateOfBirth.Create(value);

        // Assert
        Assert.True(result.IsFailure);
    }
}
