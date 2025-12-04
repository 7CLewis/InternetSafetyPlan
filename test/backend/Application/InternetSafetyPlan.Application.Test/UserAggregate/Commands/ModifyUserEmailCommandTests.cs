using InternetSafetyPlan.Application.UserAggregate.Commands;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Test.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Test.UserAggregate.Commands;

public class ModifyUserEmailCommandTests
{
    [Fact]
    public async Task ModifyUserEmail_ValidData_ReturnsSuccessfulResult()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
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

        await context.SaveChangesAsync();

        var command = new ModifyUserEmailCommand(fakeUserId, EntityTestUtils.RandomEmail());
        var handler = new ModifyUserEmailCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.GetType().Should().Be(typeof(Unit));
    }

    [Fact]
    public async Task ModifyUserEmail_InvalidData_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var random = new Faker().Random;
        var fakeUserId = random.Guid();
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

        await context.SaveChangesAsync();

        var invalidEmail = "";
        var command = new ModifyUserEmailCommand(fakeUserId, invalidEmail);
        var handler = new ModifyUserEmailCommandHandler(context);

        // Act
        var result = await handler.Handle(command, default);
        

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().NotBeNullOrEmpty();
    }
}
