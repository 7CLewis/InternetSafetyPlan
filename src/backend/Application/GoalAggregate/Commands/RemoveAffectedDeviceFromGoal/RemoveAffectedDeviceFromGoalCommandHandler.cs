using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Application.DeviceAggregate;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.GoalAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;


public class RemoveAffectedDeviceFromGoalCommandHandler : ICommandHandler<RemoveAffectedDeviceFromGoalCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public RemoveAffectedDeviceFromGoalCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(RemoveAffectedDeviceFromGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var device = await
                _databaseContext
                .Set<Device>()
                .SingleOrDefaultAsync(e => e.Id == request.DeviceId, cancellationToken);
            if (device is null) return Result.Failure<Unit>(DeviceErrors.DeviceNotFound(request.DeviceId));

            var goalWithAffectedDevices = await _databaseContext
                .Set<Goal>()
                .Include(e => e.AffectedDevices)
                .SingleOrDefaultAsync(e => e.Id == request.GoalId, cancellationToken);
            if (goalWithAffectedDevices is null) return Result.Failure<Unit>(GoalErrors.GoalNotFound(request.GoalId));

            var result = goalWithAffectedDevices.RemoveAffectedDevice(device);

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
