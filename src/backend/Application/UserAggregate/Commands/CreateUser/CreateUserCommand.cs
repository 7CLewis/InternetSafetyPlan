using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.UserAggregate.Commands;

public record CreateUserCommand(string Email) : ICommand<Guid>;

