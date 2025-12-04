using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.TeamAggregate;

namespace InternetSafetyPlan.Application.TeamAggregate;

public static class TeamErrors
{
    public static Error TeamNotFound(Guid teamId) => SharedApplicationErrors.NotFound(nameof(Team), teamId);
    public static Error TeammateNotFound(Guid teammateId) => SharedApplicationErrors.NotFound(nameof(Teammate), teammateId);
}
