using Bogus;

namespace InternetSafetyPlan.Test.Shared;

public static class EntityTestUtils
{
    public static string RandomEmail()
    {
        var random = new Faker().Random;
        return $"{random.String()}@{random.String()}.{random.String()}";
    }

    public static T RandomEnum<T>()
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Shared.Next(values.Length))!;
    }
}
