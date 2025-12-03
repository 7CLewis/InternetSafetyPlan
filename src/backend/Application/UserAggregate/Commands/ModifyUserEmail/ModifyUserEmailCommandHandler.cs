using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.UserAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.UserAggregate.Commands;

public class ModifyUserEmailCommandHandler : ICommandHandler<ModifyUserEmailCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public ModifyUserEmailCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(ModifyUserEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(emailResult.Error);
            }

            var user = await _databaseContext
                .Set<User>()
                .SingleOrDefaultAsync(e => e.Id == request.UserId, cancellationToken);
            if (user is null) return Result.Failure<Unit>(UserErrors.UserNotFound(request.UserId));

            var result = user.ModifyEmail(emailResult.Value);

            if (result.IsFailure) return Result.Failure<Unit>(result.Error);

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
