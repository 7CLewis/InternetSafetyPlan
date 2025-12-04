using InternetSafetyPlan.Application.DeviceAggregate.Queries;
using InternetSafetyPlan.Application.GoalAggregate.Queries;
using InternetSafetyPlan.Domain.TeamAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.TeamAggregate.Queries.GetTeam;

public class GetTeamQueryByIdMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Team, TeamByIdResponse>()
            .MapWith(src => new TeamByIdResponse(
                src.Id,
                src.Name.Value,
                src.Description != null ? src.Description.Value : null,
                src.Teammates.Adapt<List<TeamByIdResponse_Teammate>>())
        );

        config.NewConfig<Teammate, TeamByIdResponse_Teammate>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth != null ? src.DateOfBirth.Value : DateTime.MinValue);
    }
}
