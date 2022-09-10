using AuthServer.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AuthServer.Descriptors.Token
{
    public class ModeratorTokenDescriptor : SecurityTokenDescriptor
    {
        string _key;
        public ModeratorTokenDescriptor(string key)
        {
            _key = key;
            Configure();
        }
        private void Configure()
        {
            var key = Encoding.UTF8.GetBytes(_key);

            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Role, "Moderator"),
                });

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        }
    }
}
