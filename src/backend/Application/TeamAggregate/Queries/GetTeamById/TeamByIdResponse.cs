namespace InternetSafetyPlan.Application.TeamAggregate.Queries;

public record TeamByIdResponse(Guid Id, string Name, string? Description, List<TeamByIdResponse_Teammate> Teammates);

public record TeamByIdResponse_Teammate(Guid Id, string Name, DateTime DateOfBirth);
