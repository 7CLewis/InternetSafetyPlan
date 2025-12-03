using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.UserAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.UserAggregate.Commands;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public DeleteUserCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user =
                await _databaseContext
                .Set<User>()
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (user is null) return Result.Failure<Unit>(UserErrors.UserNotFound(request.Id));

            user.Delete();

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Unit>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
