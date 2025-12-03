using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.UserAggregate.Commands;

public record DeleteUserCommand(Guid Id) : ICommand<Unit>;
