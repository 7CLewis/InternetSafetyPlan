using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;

public record UpdateTeammateInformationCommand(Guid TeamId, Guid TeammateId, string Name, DateTime DateOfBirth, Guid? UserId = null) : ICommand<Unit>;

