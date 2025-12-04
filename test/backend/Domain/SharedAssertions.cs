namespace InternetSafetyPlan.Domain.Test;

public class SharedAssertions<T> where T : Entity
{
    /// <summary>
    /// Verify all properties are not null.
    /// This will help catch new nullable 
    /// properties that need to be tested.
    /// </summary>
    /// <param name="entity"></param>
    public static void AllPropertiesNotNull(T entity)
    {
        var type = entity.GetType();
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            Assert.NotNull(property.GetValue(entity));
        }

    }
}
