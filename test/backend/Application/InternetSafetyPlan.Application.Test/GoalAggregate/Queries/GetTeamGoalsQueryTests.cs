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

public class GetTeamGoalsQueryTests
{
    [Fact]
    public async Task GetTeamGoals_ValidTeamId_ReturnsTeamGoals()
    {
        // Arrange
        TestMapsterConfig.Initialize();

        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeUltimateGoalId = random.Guid();
        var fakeGoalId = random.Guid();
        var fakeActionItemId = random.Guid();
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

        var actionItem = ActionItem.Create(
            fakeActionItemId,
            fakeGoalId,
            ActionItemName.Create(random.String()).Value
        ).Value;

        context.Set<ActionItem>().Add(actionItem);

        await context.SaveChangesAsync();

        var query = new GetTeamGoalsQuery(fakeTeamId);
        var handler = new GetTeamGoalsQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(1);
    }

    [Fact]
    public async Task GetTeamGoals_InvalidTeamId_ReturnsEmptyList()
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

        var invalidTeamId = random.Guid();
        var query = new GetTeamGoalsQuery(invalidTeamId);
        var handler = new GetTeamGoalsQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }
}
