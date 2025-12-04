using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;

public record AddTeammateToTeamCommand(Guid TeamId, string TeammateName, DateTime TeammateDateOfBirth, Guid? UserId = null) : ICommand<Guid>;

