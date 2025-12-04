using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Application.DeviceAggregate.Commands;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.DeviceAggregate.Commands;

public class RemoveTeammateFromDeviceCommandTests
{
    [Fact]
    public async Task RemoveTeammateFromDevice_ValidTeammate_ReturnsSuccessfulResult()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeTeammateId = random.Guid();
        var fakeDeviceId = random.Guid();
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

        var device = Device.Create(
            fakeDeviceId,
            fakeTeamId,
            ManufacturerName.Create(random.String()).Value,
            Nickname.Create(random.String()).Value,
            EntityTestUtils.RandomEnum<DeviceType>()
        ).Value;

        device.AddTeammate(teammate);

        context.Set<Device>().Add(device);
        await context.SaveChangesAsync();

        var command = new RemoveTeammateFromDeviceCommand(fakeDeviceId, fakeTeammateId);
        var handler = new RemoveTeammateFromDeviceCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Unit));
    }

    [Fact]
    public async Task RemoveTeammateFromDevice_InvalidTeammate_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeTeammateId = random.Guid();
        var fakeDeviceId = random.Guid();
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

        var device = Device.Create(
            fakeDeviceId,
            fakeTeamId,
            ManufacturerName.Create(random.String()).Value,
            Nickname.Create(random.String()).Value,
            EntityTestUtils.RandomEnum<DeviceType>()
        ).Value;

        device.AddTeammate(teammate);

        context.Set<Device>().Add(device);
        await context.SaveChangesAsync();

        var nonexistentTeammateId = random.Guid();
        var command = new RemoveTeammateFromDeviceCommand(fakeDeviceId, nonexistentTeammateId);
        var handler = new RemoveTeammateFromDeviceCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedApplicationErrors.NotFound(nameof(Teammate), nonexistentTeammateId));
    }
}
