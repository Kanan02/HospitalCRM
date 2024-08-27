using Domain.Entities.Security;

namespace Application.Models.Response
{
    public class LoginRes
    {
        public string? Role { get; set; }
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Msisdn { get; set; }
        public LoginRes() { }

        public LoginRes(User user, string token)
        {
            JwtToken = token;
            Role = user?.Role?.Name;
            Msisdn = user?.Msisdn;
            RefreshToken = user?.RefreshToken;
        }
    }
}
