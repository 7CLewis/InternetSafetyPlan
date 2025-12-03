using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.UltimateGoalAggregate.Commands;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.UltimateGoalAggregate.Commands;

public class CreateUltimateGoalCommandTests
{
    [Fact]
    public async Task CreateUltimateGoal_ValidUltimateGoalData_ReturnsSuccessfulResult()
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

        var command = new CreateUltimateGoalCommand(fakeTeamId, random.String());
        var handler = new CreateUltimateGoalCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Guid));
    }

    [Fact]
    public async Task CreateUltimateGoal_InvalidUltimateGoalData_ReturnsFailureWithErrorMessage()
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

        var invalidUltimateGoalName = "";
        var command = new CreateUltimateGoalCommand(fakeTeamId, invalidUltimateGoalName);
        var handler = new CreateUltimateGoalCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedDomainErrors.Empty(nameof(UltimateGoalName)));
    }
}
