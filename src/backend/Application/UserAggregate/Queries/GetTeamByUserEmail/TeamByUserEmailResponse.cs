namespace InternetSafetyPlan.Application.UserAggregate.Queries;

public record TeamByUserEmailResponse(Guid Id, string Name, string? Description);
