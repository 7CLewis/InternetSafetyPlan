using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;

public record CreateTeamCommand(string UserEmail, string Name, string? Description = null) : ICommand<Guid>;

