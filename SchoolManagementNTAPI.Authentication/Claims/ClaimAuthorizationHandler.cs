using Microsoft.AspNetCore.Authorization;

namespace SchoolManagementNTAPI.Authentication.Claims
{
    public class ClaimAuthorizationHandler : AuthorizationHandler<ClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            var claimsFromJWT = context.User.Claims.Where(
                x => x.Type == "claims").Select(x => x.Value).ToHashSet();

            if (claimsFromJWT.Contains(requirement.Claim))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
