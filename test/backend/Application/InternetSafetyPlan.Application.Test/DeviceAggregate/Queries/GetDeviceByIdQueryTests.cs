using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.DeviceAggregate;
using InternetSafetyPlan.Application.DeviceAggregate.Queries;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.DeviceAggregate.Queries;

public class GetDeviceByIdQueryTests
{
    [Fact]
    public async Task GetDeviceById_ValidDeviceId_ReturnsCorrectDevice()
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
            Email.Create(EntityTestUtils.RandomEmail()).Value)
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
        var query = new GetDeviceByIdQuery(fakeDeviceId);
        var handler = new GetDeviceByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(fakeDeviceId);
    }

    [Fact]
    public async Task GetDeviceById_InvalidDeviceId_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;
        var unknownDeviceId = random.Guid();
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var query = new GetDeviceByIdQuery(unknownDeviceId);
        var handler = new GetDeviceByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DeviceErrors.DeviceNotFound(unknownDeviceId));
    }
}
