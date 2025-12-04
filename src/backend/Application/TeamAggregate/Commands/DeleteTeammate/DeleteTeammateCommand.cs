using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;

public record DeleteTeammateCommand(Guid TeamId, Guid TeammateId) : ICommand<Unit>;

