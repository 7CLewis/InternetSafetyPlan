using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.UserAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.UserAggregate.Queries;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserByIdResponse>
{
    private readonly IDatabaseContext _databaseContext;
    public GetUserByIdQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<UserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _databaseContext.Set<User>().SingleOrDefaultAsync(e => e.Id == request.UserId, cancellationToken);
            if (users is null) return Result.Failure<UserByIdResponse>(UserErrors.UserNotFound(request.UserId));

            var teamUsersResponse = users.Adapt<UserByIdResponse>();

            return Result.Success(teamUsersResponse);
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<UserByIdResponse>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
