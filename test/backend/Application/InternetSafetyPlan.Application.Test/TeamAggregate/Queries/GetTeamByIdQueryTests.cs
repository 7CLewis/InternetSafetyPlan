using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.TeamAggregate;
using InternetSafetyPlan.Application.TeamAggregate.Queries;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.TeamAggregate.Queries;

public class GetTeamByIdQueryTests
{
    [Fact]
    public async Task GetTeamById_ValidTeamId_ReturnsCorrectTeam()
    {
        // Arrange
        TestMapsterConfig.Initialize();

        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var user = User.Create(
            fakeUserId,
            Email.Create(EntityTestUtils.RandomEmail()).Value
        )
        .Value;

        context.Set<User>().Add(user);

        var team = Team.Create(
            fakeTeamId,
            user,
            TeamName.Create(random.String()).Value,
            Description.Create(random.String()).Value
        )
        .Value;

        context.Set<Team>().Add(team);
        await context.SaveChangesAsync();

        var query = new GetTeamByIdQuery(fakeTeamId);
        var handler = new GetTeamByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(fakeTeamId);
    }

    [Fact]
    public async Task GetTeamById_InvalidTeamId_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var invalidTeamId = random.Guid();
        var query = new GetTeamByIdQuery(invalidTeamId);
        var handler = new GetTeamByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(TeamErrors.TeamNotFound(invalidTeamId));
    }
}
