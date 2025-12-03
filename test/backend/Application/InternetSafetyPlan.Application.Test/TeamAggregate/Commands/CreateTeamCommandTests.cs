using Bogus;
using FluentAssertions;
using InternetSafetyPlan.Application.TeamAggregate.Commands;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.TeamAggregate.Commands;

public class CreateTeamCommandTests
{
    [Fact]
    public async Task CreateTeam_ValidTeamData_ReturnsSuccessfulResult()
    {
        // Arrange
        var random = new Faker().Random;
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
            Email.Create(fakeUserEmail).Value
        )
        .Value;

        context.Set<User>().Add(user);

        await context.SaveChangesAsync();

        var command = new CreateTeamCommand(fakeUserEmail, random.String());
        var handler = new CreateTeamCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Guid));
    }

    [Fact]
    public async Task CreateTeam_InvalidTeamData_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;
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
            Email.Create(fakeUserEmail).Value
        )
        .Value;

        context.Set<User>().Add(user);

        await context.SaveChangesAsync();

        var invalidTeamName = "";
        var command = new CreateTeamCommand(fakeUserEmail, invalidTeamName);
        var handler = new CreateTeamCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedDomainErrors.Empty(nameof(TeamName)));
    }
}
