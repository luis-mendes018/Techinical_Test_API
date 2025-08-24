using Microsoft.AspNetCore.Authorization;

namespace Technical_Test.API.Authorization;

public class RoleRequirement : IAuthorizationRequirement
{
    public IEnumerable<string> RequiredRoles { get; }

    public RoleRequirement(IEnumerable<string> requiredRoles)
    {
        RequiredRoles = requiredRoles;
    }
}
