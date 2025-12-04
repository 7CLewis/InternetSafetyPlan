using InternetSafetyPlan.Application.UserAggregate.Queries;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.UserAggregate.Queries;

public class GetUserByEmailQueryTests
{
    [Fact]
    public async Task GetUserByEmail_ValidUserEmail_ReturnsCorrectUser()
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

        var query = new GetUserByEmailQuery(fakeUserEmail);
        var handler = new GetUserByEmailQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(fakeUserId);
    }

    [Fact]
    public async Task GetUserByEmail_UnknownUserEmail_ReturnsNull()
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

        var unknownUserEmail = "unknown@email.com";
        var query = new GetUserByEmailQuery(unknownUserEmail);
        var handler = new GetUserByEmailQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }
}
