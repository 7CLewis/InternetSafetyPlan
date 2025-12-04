using InternetSafetyPlan.Application.UserAggregate;
using InternetSafetyPlan.Application.UserAggregate.Queries;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.UserAggregate.Queries;

public class GetUserByIdQueryTests
{
    [Fact]
    public async Task GetUserById_ValidUserId_ReturnsCorrectUser()
    {
        // Arrange
        var faker = new Faker();
        var random = faker.Random;
        var fakeUserId = random.Guid();
        var fakeUserEmail = EntityTestUtils.RandomEmail();
        var fakeTeamId = random.Guid();
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var user = User.Create(
            fakeUserId,
            Email.Create(fakeUserEmail).Value)
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

        var query = new GetUserByIdQuery(fakeUserId);
        var handler = new GetUserByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(fakeUserId);
    }

    [Fact]
    public async Task GetUserById_InvalidUserId_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var faker = new Faker();
        var random = faker.Random;
        var fakeUserId = random.Guid();
        var fakeUserEmail = EntityTestUtils.RandomEmail();
        var fakeTeamId = random.Guid();
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var user = User.Create(
            fakeUserId,
            Email.Create(fakeUserEmail).Value)
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

        var invalidUserId = random.Guid();
        var query = new GetUserByIdQuery(invalidUserId);
        var handler = new GetUserByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.UserNotFound(invalidUserId));
    }
}
