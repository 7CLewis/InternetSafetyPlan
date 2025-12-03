using InternetSafetyPlan.Domain.Base;

namespace InternetSafetyPlan.Domain.Shared;

public static class SharedDomainErrors
{
    public static Error NotFoundInList(string entityType, Guid entityId) => new($"{entityType}.NotFound", $"No {entityType} with ID {entityId} was found in the list");
    public static Error MaxCapacity(string baseEntityType, string maxEntityType, int maxCapacity) => new($"{maxEntityType}.MaxCapacity", $"This {baseEntityType} is at its max number of ${maxEntityType} ({maxCapacity}). You cannot add another until you first delete an existing one.");
    public static Error TagAlreadyAdded(string name, TagType type, string entityType) => new($"{nameof(Tag)}.AlreadyAdded", $"Tag with the Name '{name}' and Type '{type}' has already been added to this {entityType} and cannot be duplicated.");
    public static Error AlreadyAdded(string baseEntityType, string addedEntityType, Guid addedEntityId) => new($"{addedEntityType}.AlreadyAdded", $"{addedEntityType} with ID {addedEntityId} has already been added to this {baseEntityType} and cannot be duplicated.");
    public static Error Empty(string fieldName) => new($"{fieldName}.Empty", $"{fieldName} cannot be empty.");
    public static Error ExceedsMaxLength(string fieldName, int maxLength) => new($"{fieldName}.ExceedsMaxLength", $"{fieldName} exceeds the max length of {maxLength}.");
    public static Error UpdateAttemptWhenComplete(string entityType) => new($"{entityType}.UpdateWhenIsComplete", $"Cannot update {entityType} when it is marked as Completed.");
}
