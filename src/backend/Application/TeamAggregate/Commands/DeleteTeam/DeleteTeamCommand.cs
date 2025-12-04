using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;

public record DeleteTeamCommand(Guid Id) : ICommand<Unit>;
