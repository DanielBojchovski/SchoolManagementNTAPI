using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace SchoolManagementNTAPI.Authentication.JWT
{
    public record JWTModel
    {
        public string AspNetUserId { get; init; } = "";
        public string AspNetEmail { get; init; } = "";
        public HashSet<string> Roles { get; init; } = new();
        public int StudentId { get; init; }
        public List<int> Classes { get; init; } = new();

        public JWTModel()
        {

        }

        public JWTModel(JwtPayload payload)
        {
            if (payload.TryGetValue(JwtRegisteredClaimNames.Sub, out var userId))
                AspNetUserId = (string)userId;

            if (payload.TryGetValue(JwtRegisteredClaimNames.Email, out var email))
                AspNetEmail = (string)email;

            if (payload.TryGetValue("studentId", out var studentId))
                StudentId = Convert.ToInt32(studentId);

            if (payload.TryGetValue("claims", out var roles))
            {
                var deserializedRoles = JsonSerializer.Deserialize<IEnumerable<string>>(roles.ToString() ?? "");

                if (deserializedRoles is null)
                    return;

                foreach (var item in deserializedRoles)
                {
                    Roles.Add(item);
                }
            }

            if (payload.TryGetValue("classes", out var classes))
            {
                var deserializedClasses = JsonSerializer.Deserialize<IEnumerable<string>>(classes.ToString() ?? "");

                if (deserializedClasses is null)
                    return;

                foreach (var item in deserializedClasses)
                {
                    Classes.Add(Convert.ToInt32(item));
                }
            }
        }
    }
}
