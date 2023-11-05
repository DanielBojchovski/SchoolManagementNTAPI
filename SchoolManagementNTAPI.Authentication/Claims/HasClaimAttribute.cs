using Microsoft.AspNetCore.Authorization;

namespace SchoolManagementNTAPI.Authentication.Claims
{
    public class HasClaimAttribute : AuthorizeAttribute
    {
        public HasClaimAttribute(UserClaims claim)
            : base(policy: claim.ToString())
        {

        }
    }
}
