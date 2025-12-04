using InternetSafetyPlan.Domain.UserAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.UserAggregate.Queries;

public class GetTeamByUserEmailQueryMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserByEmailResponse>()
            .Map(dest => dest.Email, src => src.Email.Value);
    }
}
