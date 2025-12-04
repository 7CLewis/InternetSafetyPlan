using InternetSafetyPlan.Domain.TeamAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.TeamAggregate.Queries.GetTeam;

public class GetTeammateQueryByIdMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Teammate, TeammateByIdResponse>()
            .MapWith(src => new TeammateByIdResponse(
                src.Id,
                src.Name.Value,
                src.DateOfBirth != null ? src.DateOfBirth.Value : null
        ));
    }
}
