using Microsoft.AspNetCore.Authorization;

namespace SchoolManagementNTAPI.Authentication.Claims
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public ClaimRequirement(string claim)
        {
            Claim = claim;
        }

        public string Claim { get; }
    }
}
