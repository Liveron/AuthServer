using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using AuthServer.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Authorization.Descriptors.Token
{
    public class UserTokenDescriptor : SecurityTokenDescriptor
    {
        private readonly AppUser _user;
        private readonly IConfiguration _configuration;
        public UserTokenDescriptor(AppUser user, IConfiguration configuration)
        {
            _user = user;
            _configuration = configuration;
            Configure();
        }
        private void Configure()
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            Subject = new ClaimsIdentity(
                new[] 
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                });

            Expires = DateTime.UtcNow.AddDays(7);
            Audience = "User";
            Issuer = "AuthServer";

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        }
    }
}
