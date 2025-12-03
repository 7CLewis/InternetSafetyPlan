using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Commands;

public record CreateUltimateGoalCommand(Guid TeamId, string Name, string? Description = null) : ICommand<Guid>;
