namespace SchoolManagementNTAPI.Authentication.Claims
{
    public static class ClaimPermissionType
    {
        public const string Permission = "Permission";
    }
    public enum UserClaims
    {
        Admin = 1,
        Principal,
        Professor,
        Student
    }
}
