using Bogus;
using FluentAssertions;
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

public class AddTagToDeviceCommandTests
{
    [Fact]
    public async Task AddTagToDevice_ValidTag_ReturnsSuccessfulResult()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeDeviceId = random.Guid();
        var fakeTagId = random.Guid();
        var fakeTagName = random.String2(25);
        var fakeTagType = EntityTestUtils.RandomEnum<TagType>();
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
        ).Value;

        context.Set<Team>().Add(team);

        var device = Device.Create(
            fakeDeviceId,
            fakeTeamId,
            ManufacturerName.Create(random.String()).Value,
            Nickname.Create(random.String()).Value,
            EntityTestUtils.RandomEnum<DeviceType>()
        ).Value;

        context.Set<Device>().Add(device);

        var tag = Tag.Create(
            fakeTagId,
            fakeTagName,
            fakeTagType
        ).Value;

        context.Set<Tag>().Add(tag);
        await context.SaveChangesAsync();

        var command = new AddTagToDeviceCommand(fakeDeviceId, fakeTagName, fakeTagType);
        var handler = new AddTagToDeviceCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Unit));
    }

    [Fact]
    public async Task AddTagToDevice_NonexistentTag_ReturnsFailureWithErrorMessage()
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
        ).Value;

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

        var nonexistentTagName = "";
        var command = new AddTagToDeviceCommand(fakeDeviceId, nonexistentTagName, EntityTestUtils.RandomEnum<TagType>());
        var handler = new AddTagToDeviceCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task AddTagToDevice_NonexistentDevice_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
        var fakeTeamId = random.Guid();
        var fakeDeviceId = random.Guid();
        var fakeTagId = random.Guid();
        var fakeTagName = random.String();
        var fakeTagType = EntityTestUtils.RandomEnum<TagType>();
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var nonexistentDeviceId = Guid.NewGuid();
        var command = new AddTagToDeviceCommand(nonexistentDeviceId, random.String2(25), EntityTestUtils.RandomEnum<TagType>());
        var handler = new AddTagToDeviceCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().NotBeNullOrEmpty();
    }
}
