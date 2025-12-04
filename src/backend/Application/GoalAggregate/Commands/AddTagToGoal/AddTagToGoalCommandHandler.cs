using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;


public class AddTagToGoalCommandHandler : ICommandHandler<AddTagToGoalCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public AddTagToGoalCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(AddTagToGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tag =
                await _databaseContext
                .Set<Tag>()
                .SingleOrDefaultAsync(e => e.Name == request.TagName && e.Type == request.TagType, cancellationToken);
            if (tag is null) return Result.Failure<Unit>(SharedApplicationErrors.TagNotFound(request.TagName, request.TagType));

            var goalWithTags = await _databaseContext
                .Set<Goal>()
                .Include(e => e.Tags)
                .SingleOrDefaultAsync(e => e.Id == request.GoalId, cancellationToken);
            if (goalWithTags is null) return Result.Failure<Unit>(GoalErrors.GoalNotFound(request.GoalId));

            var result = goalWithTags.AddTag(tag);

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
