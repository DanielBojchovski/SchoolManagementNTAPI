namespace SchoolManagementNTAPI.Authentication.Claims
{
    public static class ClaimPermissionType
    {
        public const string Permission = "Permission";
    }
    public enum UserClaims
    {
        User = 1,
        Admin,
        Developer
    }
}
