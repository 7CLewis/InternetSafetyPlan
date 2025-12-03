using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace InternetSafetyPlan.Application.Base;

public static class MapsterConfiguration
{
#pragma warning disable IDE0060 // Remove unused parameter.
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
#pragma warning restore IDE0060 // Remove unused parameter.
}
