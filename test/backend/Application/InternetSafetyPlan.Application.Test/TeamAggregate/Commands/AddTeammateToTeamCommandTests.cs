using InternetSafetyPlan.Application.TeamAggregate.Commands;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.TeamAggregate.Commands;

public class AddTeammateToTeamCommandTests
{
    [Fact]
    public async Task AddTeammateToTeam_ValidTeammateData_ReturnsSuccess()
    {
        // Arrange
        var faker = new Faker();
        var random =faker.Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var user = User.Create(
            fakeUserId,
            Email.Create(EntityTestUtils.RandomEmail()).Value)
            .Value;

        context.Set<User>().Add(user);

        var team = Team.Create(
            fakeTeamId,
            user,
            TeamName.Create(random.String()).Value,
            Description.Create(random.String()).Value)
            .Value;

        context.Set<Team>().Add(team);

        await context.SaveChangesAsync();

        var command = new AddTeammateToTeamCommand(fakeTeamId, random.String(), faker.Date.Past(1));
        var handler = new AddTeammateToTeamCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Guid));
    }

    [Fact]
    public async Task AddTeammate_TeammateWithEmptyName_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var faker = new Faker();
        var random = faker.Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var user = User.Create(
            fakeUserId,
            Email.Create(EntityTestUtils.RandomEmail()).Value)
            .Value;

        context.Set<User>().Add(user);

        var team = Team.Create(
            fakeTeamId,
            user,
            TeamName.Create(random.String()).Value,
            Description.Create(random.String()).Value)
            .Value;

        context.Set<Team>().Add(team);

        await context.SaveChangesAsync();

        var invalidNameString = "";
        var command = new AddTeammateToTeamCommand(fakeTeamId, invalidNameString, faker.Date.Past(1));
        var handler = new AddTeammateToTeamCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedDomainErrors.Empty(nameof(TeammateName)));
    }
}
