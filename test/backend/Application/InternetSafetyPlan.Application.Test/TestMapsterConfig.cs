using Mapster;

namespace InternetSafetyPlan.Application.Test;

public class TestMapsterConfig
{
    private static readonly object _lock = new();
    private static bool _configInitialized;

    public static void Initialize()
    {
        lock (_lock)
        {
            if (!_configInitialized)
            {
                TypeAdapterConfig.GlobalSettings.Scan(typeof(AssemblyReference).Assembly);
                _configInitialized = true;
            }
        }
    }
}
