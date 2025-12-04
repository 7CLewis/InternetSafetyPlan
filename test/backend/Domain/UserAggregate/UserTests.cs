namespace InternetSafetyPlan.Domain.Test.UserAggregate;

public class UserTests
{
    private readonly Guid UserId = Guid.NewGuid();
    private readonly Email Email = Email.Create("c@gmail.com").Value;
    private readonly Guid TeamId = Guid.NewGuid();

    [Fact]
    public void Create_Should_SetAllProperties()
    {
        // Arrange

        // Act
        var result = User.Create(UserId, Email, TeamId);
        var user = result.Value;


        // Assert
        Assert.True(result.IsSuccess);
        SharedAssertions<User>.AllPropertiesNotNull(user);
        Assert.Equal(Email, user.Email);
    }

    [Fact]
    public void ModifyEmail_Should_UpdateEmail()
    {
        // Arrange
        var originalEmail = Email.Create("b@gmail.com").Value;
        var updatedEmail = Email;
        var user = User.Create(UserId, originalEmail).Value;

        // Act
        var result = user.ModifyEmail(updatedEmail);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Email, user.Email);
    }
}
