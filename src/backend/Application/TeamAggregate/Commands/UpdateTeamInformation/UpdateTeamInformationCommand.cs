using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;

public record UpdateTeamInformationCommand(Guid TeamId, string Name, string Description) : ICommand<Unit>;

