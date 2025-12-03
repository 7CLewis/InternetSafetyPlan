using InternetSafetyPlan.Application.UserAggregate.Commands;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.UserAggregate.Commands;

public class CreateUserCommandTest
{
    [Fact]
    public async Task CreateUser_ValidUserData_ReturnsSuccessfulResult()
    {
        // Arrange
        var random = new Faker().Random;
        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var command = new CreateUserCommand(random.String());
        var handler = new CreateUserCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Guid));
    }

    [Fact]
    public async Task CreateUser_InvalidUserData_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;

        using var context = new DatabaseContext(
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(random.String())
                .Options
        );

        var invalidEmail = "";
        var command = new CreateUserCommand(invalidEmail);
        var handler = new CreateUserCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SharedDomainErrors.Empty(nameof(Email)));
    }
}
