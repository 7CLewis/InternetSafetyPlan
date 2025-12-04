namespace InternetSafetyPlan.Domain.Test.TeamAggregate;
public class TeammateTests
{
    private readonly Guid TeammateId = Guid.NewGuid();
    private readonly Guid TeamId = Guid.NewGuid();
    private readonly Guid UserId = Guid.NewGuid();
    private readonly TeammateName Name = TeammateName.Create("Kayla").Value;
    private readonly DateOfBirth DateOfBirth = DateOfBirth.Create(new DateTime(1989, 12, 13)).Value;

    [Fact]
    public void Create_Should_SetAllProperties()
    {
        // Arrange

        // Act
        var result = Teammate.Create(TeammateId, TeamId, Name, DateOfBirth, UserId);
        var teammate = result.Value;


        // Assert
        Assert.True(result.IsSuccess);
        SharedAssertions<Teammate>.AllPropertiesNotNull(teammate);
        Assert.Equal(TeamId, teammate.TeamId);
        Assert.Equal(Name, teammate.Name);
        Assert.Equal(DateOfBirth, teammate.DateOfBirth);
    }

    [Fact]
    public void UpdateInformation_Should_UpdateSpecifiedProperties()
    {
        // Arrange
        var originalTeammateName = TeammateName.Create("Kaylw").Value;

        var updatedTeammateName = Name;

        var createResult = Teammate.Create(TeammateId, TeamId, originalTeammateName, DateOfBirth, UserId);
        var teammate = createResult.Value;

        // Act
        var updateResult = teammate.UpdateInformation(updatedTeammateName, DateOfBirth, UserId);

        // Assert
        Assert.True(updateResult.IsSuccess);
        Assert.Equal(TeamId, teammate.TeamId);
        Assert.Equal(Name, teammate.Name);
        Assert.Equal(DateOfBirth, teammate.DateOfBirth);
    }
}
