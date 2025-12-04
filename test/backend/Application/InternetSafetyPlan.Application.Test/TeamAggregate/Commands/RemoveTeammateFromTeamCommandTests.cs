using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Application.TeamAggregate.Commands;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.TeamAggregate.Commands;

public class RemoveTeammateFromTeamCommandTests
{
    [Fact]
    public async Task RemoveTeammateFromTeam_ValidTeammate_ReturnsSuccessfulResult()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeTeammateId = random.Guid();
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

        var teammate = Teammate.Create(
            fakeTeammateId,
            fakeTeamId,
            TeammateName.Create(random.String()).Value
        )
        .Value;

        context.Set<Teammate>().Add(teammate);
        await context.SaveChangesAsync();

        var command = new DeleteTeammateCommand(fakeTeamId, fakeTeammateId);
        var handler = new DeleteTeammateCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Unit));
    }

    [Fact]
    public async Task DeleteTeammate_InvalidTeammate_ReturnsFailureWithErrorMessage()
    {
        // Arrange
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

        var nonexistentTeammateId = random.Guid();
        var command = new DeleteTeammateCommand(fakeTeamId, nonexistentTeammateId);
        var handler = new DeleteTeammateCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedApplicationErrors.NotFound(nameof(Teammate), nonexistentTeammateId));
    }
}
