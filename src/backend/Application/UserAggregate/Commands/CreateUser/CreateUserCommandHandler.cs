using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.UserAggregate;

namespace InternetSafetyPlan.Application.UserAggregate.Commands;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IDatabaseContext _databaseContext;
    public CreateUserCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure)
            {
                // Log error
                return Result.Failure<Guid>(emailResult.Error);
            }

            var userId = Guid.NewGuid();

            var userResult = User.Create(userId, emailResult.Value);
            if (userResult.IsFailure) return Result.Failure<Guid>(userResult.Error);

            var user = userResult.Value;

            await _databaseContext.Set<User>().AddAsync(user, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return userId;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Guid>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
