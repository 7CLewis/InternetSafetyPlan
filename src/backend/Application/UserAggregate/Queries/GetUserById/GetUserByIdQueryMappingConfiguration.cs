using InternetSafetyPlan.Domain.UserAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.UserAggregate.Queries;

public class GetUserByIdQueryMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserByIdResponse>()
            .Map(dest => dest.Email, src => src.Email.Value);
    }
}
