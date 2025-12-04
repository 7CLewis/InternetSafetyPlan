using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate;

public static class UltimateGoalErrors
{
    public static Error UltimateGoalNotFound(Guid ultimateGoalId) => SharedApplicationErrors.NotFound(nameof(UltimateGoal), ultimateGoalId);
}
