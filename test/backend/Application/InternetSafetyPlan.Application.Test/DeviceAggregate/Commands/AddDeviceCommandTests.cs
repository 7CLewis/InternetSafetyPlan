using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.DeviceAggregate.Commands;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.DeviceAggregate.Commands;

public class AddDeviceCommandTests
{
    [Fact]
    public async Task AddDevice_ValidDevice_ReturnsDeviceId()
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

        var command = new AddDeviceCommand(fakeTeamId, random.String(), random.String(), DeviceType.Phone, [], []);
        var handler = new AddDeviceCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Guid));
    }

    [Fact]
    public async Task AddDevice_DeviceWithEmptyName_ReturnsFailureWithErrorMessage()
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

        var invalidNameString = "";
        var command = new AddDeviceCommand(fakeTeamId, invalidNameString, random.String(), DeviceType.Phone, [], []);
        var handler = new AddDeviceCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedDomainErrors.Empty(nameof(ManufacturerName)));
    }

    [Fact]
    public async Task AddDevice_DeviceWithInvalidNickname_ReturnsFailureWithErrorMessage()
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

        var invalidNicknameString = "";
        var command = new AddDeviceCommand(fakeTeamId, random.String(), invalidNicknameString, DeviceType.Phone, [], []);
        var handler = new AddDeviceCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedDomainErrors.Empty(nameof(Nickname)));
    }
}
