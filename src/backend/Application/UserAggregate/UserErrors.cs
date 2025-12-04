using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.UserAggregate;

namespace InternetSafetyPlan.Application.UserAggregate;

public static class UserErrors
{
    public static Error UserNotFound(Guid userId) => SharedApplicationErrors.NotFound(nameof(User), userId);
    public static Error UserNotFoundByEmail(string email) => new("User.NotFoundByEmail", "No user was found with the email " + email);
}
