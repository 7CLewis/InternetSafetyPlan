namespace InternetSafetyPlan.Domain.Test.TeamAggregate;

public class TeamTests
{
    private readonly User User = User.Create(Guid.NewGuid(), Email.Create("test@i.com").Value).Value;
    private readonly Guid TeamId = Guid.NewGuid();
    private readonly TeamName Name = TeamName.Create("Wildcats").Value;
    private readonly Description Description = Description.Create("What team? WILDCATS!").Value;

    [Fact]
    public void Create_Should_SetAllProperties()
    {
        // Arrange

        // Act
        var team = Team.Create(TeamId, User, Name, Description).Value;

        // Assert
        SharedAssertions<Team>.AllPropertiesNotNull(team);
        Assert.Equal(team.Name, Name);
        Assert.Equal(team.Description, Description);
    }

    [Fact]
    public void UpdateInformation_Should_UpdateSpecifiedProperties()
    {
        // Arrange
        var originalName = TeamName.Create("Bobcats").Value;
        var originalDescription = Description.Create("Who dey?").Value;

        var updatedName = Name;
        var updatedDescription = Description;

        var team = Team.Create(TeamId, User, originalName, originalDescription).Value;

        // Act
        var updateResult = team.UpdateInformation(updatedName, updatedDescription);

        // Assert
        Assert.True(updateResult.IsSuccess);
        Assert.Equal(team.Name, updatedName);
        Assert.Equal(team.Description, updatedDescription);
    }

    [Fact]
    public void AddTeammate_Should_SuccessfullyAddTeammate_WhenTeammateIsNotInList()
    {
        // Arrange
        var teammate = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Charles").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var team = Team.Create(TeamId, User, Name, Description).Value;

        // Act
        var addResult = team.AddTeammate(teammate);

        // Assert
        Assert.True(addResult.IsSuccess);
        Assert.Single(team.Teammates);
        Assert.Equal(teammate, team.Teammates.Single());
    }

    [Fact]
    public void AddTeammate_Should_ReturnFailure_WhenTeammateIsInList()
    {
        // Arrange
        var teammate = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Charles").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var team = Team.Create(TeamId, User, Name, Description).Value;

        // Act
        var addResult1 = team.AddTeammate(teammate);
        var addResult2 = team.AddTeammate(teammate);

        // Assert
        Assert.True(addResult1.IsSuccess);
        Assert.True(addResult2.IsFailure);
        Assert.Equal(teammate, team.Teammates.Single());
    }

    [Fact]
    public void AddTeammate_Should_ReturnFailure_WhenTeammateListIsAtCapacity()
    {
        // Arrange
        var team = Team.Create(TeamId, User, Name, Description).Value;

        // Act
        List<Result<Unit>> addResults = [];
        for (var i = 0; i < Team.TeammateCapacity; i++)
        {
            var teammate = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Sir Charles " + i.ToString()).Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
            addResults.Add(team.AddTeammate(teammate));
        }

        var finalAdd = team.AddTeammate(Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Sir Charles").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value);

        // Assert
        addResults.ForEach(result => Assert.True(result.IsSuccess));
        Assert.True(finalAdd.IsFailure);
        Assert.Equal(Team.TeammateCapacity, team.Teammates.Count());
    }

    [Fact]
    public void DeleteTeammate_Should_SuccessfullyDeleteTeammate_WhenTagIsInList()
    {
        // Arrange
        var teammate1 = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Charles").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var teammate2 = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Carls").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var team = Team.Create(TeamId, User, Name, Description).Value;
        team.AddTeammate(teammate1);
        team.AddTeammate(teammate2);

        // Act
        var removeResult = team.DeleteTeammate(teammate1);

        // Assert
        Assert.True(removeResult.IsSuccess);
    }

    [Fact]
    public void DeleteTeammate_Should_ReturnFailure_WhenTeammateIsNotInList()
    {
        // Arrange
        var teammate1 = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Charles").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var teammate2 = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Carls").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var team = Team.Create(TeamId, User, Name, Description).Value;
        team.AddTeammate(teammate1);

        // Act
        var removeResult = team.DeleteTeammate(teammate2);

        // Assert
        Assert.True(removeResult.IsFailure);
        Assert.Single(team.Teammates);
        Assert.Equal(teammate1, team.Teammates.Single());
    }

    [Fact]
    public void AddUltimateGoal_Should_SuccessfullyAddUltimateGoal_WhenUltimateGoalIsNotInList()
    {
        // Arrange
        var team = Team.Create(TeamId, User, Name, Description).Value;
        var ultimateGoal = UltimateGoal.Create(Guid.NewGuid(), TeamId, UltimateGoalName.Create("Raise internet-safe kids").Value, Description).Value;

        // Act
        var addResult = team.AddUltimateGoal(ultimateGoal);

        // Assert
        Assert.True(addResult.IsSuccess);
        Assert.Single(team.UltimateGoals);
        Assert.Equal(ultimateGoal, team.UltimateGoals.Single());
    }

    [Fact]
    public void AddUltimateGoal_Should_ReturnFailure_WhenUltimateGoalIsInList()
    {
        // Arrange
        var team = Team.Create(TeamId, User, Name, Description).Value;
        var ultimateGoal = UltimateGoal.Create(Guid.NewGuid(), TeamId, UltimateGoalName.Create("Raise internet-safe kids").Value, Description).Value;

        // Act
        var addResult1 = team.AddUltimateGoal(ultimateGoal);
        var addResult2 = team.AddUltimateGoal(ultimateGoal);

        // Assert
        Assert.True(addResult1.IsSuccess);
        Assert.True(addResult2.IsFailure);
        Assert.Equal(ultimateGoal, team.UltimateGoals.Single());
    }

    [Fact]
    public void AddUltimateGoal_Should_ReturnFailure_WhenUltimateGoalListIsAtCapacity()
    {
        // Arrange
        var team = Team.Create(TeamId, User, Name, Description).Value;

        // Act
        List<Result<Unit>> addResults = [];
        for (var i = 0; i < Team.UltimateGoalCapacity; i++)
        {
            var ultimateGoal = UltimateGoal.Create(Guid.NewGuid(), TeamId, UltimateGoalName.Create($"Raise {i} internet-safe kids").Value, Description).Value;
            addResults.Add(team.AddUltimateGoal(ultimateGoal));
        }

        var finalUltimateGoal = UltimateGoal.Create(Guid.NewGuid(), TeamId, UltimateGoalName.Create("Raise internet-safe kids").Value, Description).Value;
        var finalAddResult = team.AddUltimateGoal(finalUltimateGoal);

        // Assert
        addResults.ForEach(result => Assert.True(result.IsSuccess));
        Assert.True(finalAddResult.IsFailure);
        Assert.Equal(Team.UltimateGoalCapacity, team.UltimateGoals.Count());
    }

    [Fact]
    public void RemoveUltimateGoal_Should_SuccessfullyRemoveUltimateGoal_WhenUserIsInList()
    {
        // Arrange
        var team = Team.Create(TeamId, User, Name, Description).Value;
        var ultimateGoal1 = UltimateGoal.Create(Guid.NewGuid(), TeamId, UltimateGoalName.Create("Raise internet-safe kids").Value, Description).Value;
        var ultimateGoal2 = UltimateGoal.Create(Guid.NewGuid(), TeamId, UltimateGoalName.Create("Be internet-safe parents").Value, Description).Value;

        team.AddUltimateGoal(ultimateGoal1);
        team.AddUltimateGoal(ultimateGoal2);

        // Act
        var removeResult = team.RemoveUltimateGoal(ultimateGoal1);

        // Assert
        Assert.True(removeResult.IsSuccess);
        Assert.Single(team.UltimateGoals);
        Assert.Equal(ultimateGoal2, team.UltimateGoals.Single());
    }

    [Fact]
    public void RemoveUltimateGoal_Should_ReturnFailure_WhenUltimateGoalIsNotInList()
    {
        // Arrange
        var team = Team.Create(TeamId, User, Name, Description).Value;
        var ultimateGoal1 = UltimateGoal.Create(Guid.NewGuid(), TeamId, UltimateGoalName.Create("Raise internet-safe kids").Value, Description).Value;
        var ultimateGoal2 = UltimateGoal.Create(Guid.NewGuid(), TeamId, UltimateGoalName.Create("Be internet-safe parents").Value, Description).Value;

        team.AddUltimateGoal(ultimateGoal1);

        // Act
        var remove = team.RemoveUltimateGoal(ultimateGoal2);

        // Assert
        Assert.True(remove.IsFailure);
        Assert.Single(team.UltimateGoals);
        Assert.Equal(ultimateGoal1, team.UltimateGoals.Single());
    }
}
