using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.UserAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.UserAggregate.Queries;

public class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, UserByEmailResponse?>
{
    private readonly IDatabaseContext _databaseContext;
    public GetUserByEmailQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<UserByEmailResponse?>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _databaseContext
                .Set<User>()
                .SingleOrDefaultAsync(e => e.Email.Value == request.Email, cancellationToken);
            if (user is null) return Result.Success<UserByEmailResponse?>(null);

            var userResponse = user.Adapt<UserByEmailResponse>();

            return userResponse;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<UserByEmailResponse?>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
