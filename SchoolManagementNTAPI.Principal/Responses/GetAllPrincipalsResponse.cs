using SchoolManagementNTAPI.Principal.Models;

namespace SchoolManagementNTAPI.Principal.Responses
{
    public class GetAllPrincipalsResponse
    {
        public List<PrincipalModel> Lista { get; set; } = new();
    }
}
