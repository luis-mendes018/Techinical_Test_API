using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Technical_Test.API.Authorization;

public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        var userRoles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value);

        foreach (var requiredRole in requirement.RequiredRoles)
        {
            if (userRoles.Contains(requiredRole))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }

        context.Fail();
        return Task.CompletedTask;
    }
}
