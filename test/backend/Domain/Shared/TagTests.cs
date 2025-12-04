namespace InternetSafetyPlan.Domain.Test.Shared;

public class TagTests
{
    private readonly TagType TagType = TagType.Application;

    [Fact]
    public void Create_SuccessfullySetsValue()
    {
        // Arrange
        var nameValue = new string('c', Tag.NameMaxLength);

        // Act
        var result = Tag.Create(Guid.NewGuid(), nameValue, TagType);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(nameValue, result.Value.Name);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsLongerThanMaxLength()
    {
        // Arrange
        var nameValue = new string('c', Tag.NameMaxLength + 1);

        // Act
        var result = Tag.Create(Guid.NewGuid(), nameValue, TagType);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsEmpty()
    {
        // Arrange
        var nameValue = "";

        // Act
        var result = Tag.Create(Guid.NewGuid(), nameValue, TagType);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ReturnsFailureResult_WhenValueIsWhitespaceOnly()
    {
        // Arrange
        var nameValue = "        ";

        // Act
        var result = Tag.Create(Guid.NewGuid(), nameValue, TagType);

        // Assert
        Assert.True(result.IsFailure);
    }
}
