using InternetSafetyPlan.Application.UserAggregate.Queries;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.UserAggregate.Queries;

public class GetTeamByUserEmailQueryTests
{
    [Fact]
    public async Task GetTeamByUserEmail_ValidUserEmail_ReturnsTeamByUserEmail()
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

        var query = new GetTeamByUserEmailQuery(fakeUserEmail);
        var handler = new GetTeamByUserEmailQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(fakeTeamId);
    }

    [Fact]
    public async Task GetTeamByUserEmail_UnknownUserEmail_ReturnsEmptyList()
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
        var query = new GetTeamByUserEmailQuery(unknownUserEmail);
        var handler = new GetTeamByUserEmailQueryHandler(context);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}
