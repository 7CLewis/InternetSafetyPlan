namespace InternetSafetyPlan.Domain.Test.UltimateGoalAggregate;

public class UltimateGoalTests
{
    private readonly Guid UltimateGoalId = Guid.NewGuid();
    private readonly Guid TeamId = Guid.NewGuid();
    private readonly User User = User.Create(Guid.NewGuid(), Email.Create("test@i.com").Value).Value;
    private readonly UltimateGoalName Name = UltimateGoalName.Create("Raise internet-safe children").Value;
    private readonly Description Description = Description.Create("Lead our children to be internet-safe their whole lives").Value;

    [Fact]
    public void Create_Should_SetAllProperties()
    {
        // Arrange
        var team = Team.Create(TeamId, User, TeamName.Create("Wildcats").Value).Value;

        // Act
        var ultimateGoal = UltimateGoal.Create(UltimateGoalId, TeamId, Name, Description).Value;

        // Assert
        SharedAssertions<UltimateGoal>.AllPropertiesNotNull(ultimateGoal);
        Assert.Equal(ultimateGoal.Name, Name);
        Assert.Equal(ultimateGoal.Description, Description);
    }

    [Fact]
    public void UpdateInformation_Should_UpdateSpecifiedProperties()
    {
        // Arrange
        var team = Team.Create(TeamId, User, TeamName.Create("Wildcats").Value).Value;
        var originalName = UltimateGoalName.Create("Rise internet-safe kids.").Value;
        var originalDescription = Description.Create("Lead 'em").Value;

        var updatedName = Name;
        var updatedDescription = Description;

        var ultimateGoal = UltimateGoal.Create(UltimateGoalId, TeamId, originalName, originalDescription).Value;

        // Act
        var updateResult = ultimateGoal.UpdateInformation(updatedName, updatedDescription);

        // Assert
        Assert.True(updateResult.IsSuccess);
        Assert.Equal(ultimateGoal.Name, updatedName);
        Assert.Equal(ultimateGoal.Description, updatedDescription);
    }
}
