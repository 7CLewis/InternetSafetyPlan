namespace InternetSafetyPlan.Application.TeamAggregate.Queries;

public record TeammateByIdResponse(Guid Id, string Name, DateTime? DateOfBirth);
