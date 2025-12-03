using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.GoalAggregate;
using InternetSafetyPlan.Application.GoalAggregate.Queries;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.GoalAggregate.Queries;

public class GetGoalByIdQueryTests
{
    [Fact]
    public async Task GetGoalById_ValidGoalId_ReturnsCorrectGoal()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeUltimateGoalId = random.Guid();
        var fakeGoalId = random.Guid();
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

        var ultimateGoal = UltimateGoal.Create(
            fakeUltimateGoalId,
            fakeTeamId,
            UltimateGoalName.Create(random.String()).Value
        ).Value;

        context.Set<UltimateGoal>().Add(ultimateGoal);

        var goal = Goal.Create(
            fakeGoalId,
            fakeUltimateGoalId,
            GoalName.Create(random.String()).Value,
            EntityTestUtils.RandomEnum<GoalCategory>()
        ).Value;

        context.Set<Goal>().Add(goal);

        await context.SaveChangesAsync();

        var query = new GetGoalByIdQuery(fakeGoalId);
        var handler = new GetGoalByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(fakeGoalId);
    }

    [Fact]
    public async Task GetGoalById_InvalidGoalId_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var invalidGoalId = random.Guid();
        var query = new GetGoalByIdQuery(invalidGoalId);
        var handler = new GetGoalByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(GoalErrors.GoalNotFound(invalidGoalId));
    }
}
