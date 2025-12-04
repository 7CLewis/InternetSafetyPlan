using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.DeviceAggregate.Queries;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.DeviceAggregate.Queries;

public class GetTeamDevicesQueryTests
{
    [Fact]
    public async Task GetTeamDevices_ValidTeamId_ReturnsTeamDevices()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
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

        var device = Device.Create(
            fakeDeviceId,
            fakeTeamId,
            ManufacturerName.Create(random.String()).Value,
            Nickname.Create(random.String()).Value,
            EntityTestUtils.RandomEnum<DeviceType>()
        ).Value;

        context.Set<Device>().Add(device);
        await context.SaveChangesAsync();

        var query = new GetTeamDevicesQuery(fakeTeamId);
        var handler = new GetTeamDevicesQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(1);
    }

    [Fact]
    public async Task GetTeamDevices_UnknownTeamId_ReturnsEmptyList()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
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

        var device = Device.Create(
            fakeDeviceId,
            fakeTeamId,
            ManufacturerName.Create(random.String()).Value,
            Nickname.Create(random.String()).Value,
            EntityTestUtils.RandomEnum<DeviceType>()
        ).Value;

        context.Set<Device>().Add(device);
        await context.SaveChangesAsync();

        var unknownTeamId = random.Guid();
        var query = new GetTeamDevicesQuery(unknownTeamId);
        var handler = new GetTeamDevicesQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }
}
