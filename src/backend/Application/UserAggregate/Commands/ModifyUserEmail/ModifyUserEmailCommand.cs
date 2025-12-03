using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.UserAggregate.Commands;

public record ModifyUserEmailCommand(Guid UserId, string Email) : ICommand<Unit>;
