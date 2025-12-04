using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;

namespace InternetSafetyPlan.Application.DeviceAggregate;

public static class DeviceErrors
{
    public static Error DeviceNotFound(Guid deviceId) => SharedApplicationErrors.NotFound(nameof(Device), deviceId);
}
