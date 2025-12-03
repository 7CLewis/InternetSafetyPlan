using System;
using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Application.GoalAggregate.Commands;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.GoalAggregate.Commands;

public class AddAffectedGoalToGoalCommandTests
{
    [Fact]
    public async Task AddAffectedDeviceToGoal_ValidGoal_ReturnsSuccessfulResult()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeDeviceId = random.Guid();
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

        var device = Device.Create(
            fakeDeviceId,
            fakeTeamId,
            ManufacturerName.Create(random.String()).Value,
            Nickname.Create(random.String()).Value,
            EntityTestUtils.RandomEnum<DeviceType>()
        ).Value;

        context.Set<Device>().Add(device);

        context.Set<UltimateGoal>().Add(ultimateGoal);

        var goal = Goal.Create(
            fakeGoalId,
            fakeUltimateGoalId,
            GoalName.Create(random.String()).Value,
            EntityTestUtils.RandomEnum<GoalCategory>()
        ).Value;

        context.Set<Goal>().Add(goal);

        await context.SaveChangesAsync();

        var command = new AddAffectedDeviceToGoalCommand(fakeGoalId, fakeDeviceId);
        var handler = new AddAffectedDeviceToGoalCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Unit));
    }

    [Fact]
    public async Task AddAffectedDeviceToGoal_NonexistentDevice_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeDeviceId = random.Guid();
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

        await context.SaveChangesAsync();

        var nonexistentDeviceId = random.Guid();
        var command = new AddAffectedDeviceToGoalCommand(fakeGoalId, nonexistentDeviceId);
        var handler = new AddAffectedDeviceToGoalCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedApplicationErrors.NotFound(nameof(Device), nonexistentDeviceId));
    }

    [Fact]
    public async Task AddAffectedDeviceToGoal_NonexistentGoal_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeDeviceId = random.Guid();
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

        var device = Device.Create(
            fakeDeviceId,
            fakeTeamId,
            ManufacturerName.Create(random.String()).Value,
            Nickname.Create(random.String()).Value,
            EntityTestUtils.RandomEnum<DeviceType>()
        ).Value;

        context.Set<Device>().Add(device);

        await context.SaveChangesAsync();

        var nonexistentGoalId = random.Guid();
        var command = new AddAffectedDeviceToGoalCommand(nonexistentGoalId, fakeDeviceId);
        var handler = new AddAffectedDeviceToGoalCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedApplicationErrors.NotFound(nameof(Goal), nonexistentGoalId));
    }
}
