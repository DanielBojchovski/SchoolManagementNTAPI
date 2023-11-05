namespace SchoolManagementNTAPI.AppUser.Responses
{
    public class UserDropDownResponse
    {
        public List<UserDropDownItem> Lista { get; set; } = new();
    }
    public class UserDropDownItem
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Disabled { get; set; }
    }
}
