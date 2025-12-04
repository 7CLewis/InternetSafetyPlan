using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Application.Base;

public static class SharedApplicationErrors
{
    public static Error Database(string message) => new("DatabaseError", "A database error occurred. " + message);
    public static Error NotFound(string entityType, Guid entityId) => new($"{entityType}.NotFound", $"No {entityType} with ID {entityId} was found");
    public static Error TagNotFound(string name, TagType type) => new($"{nameof(Tag)}.NotFound", $"No tag with Name '{name}' and Type '{type}' was found.");
}
