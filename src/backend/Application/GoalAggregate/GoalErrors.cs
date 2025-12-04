using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;

namespace InternetSafetyPlan.Application.GoalAggregate;

public static class GoalErrors
{
    public static Error GoalNotFound(Guid goalId) => SharedApplicationErrors.NotFound(nameof(Goal), goalId);
    public static Error ActionItemNotFound(Guid actionItemId) => SharedApplicationErrors.NotFound(nameof(ActionItem), actionItemId);
}
