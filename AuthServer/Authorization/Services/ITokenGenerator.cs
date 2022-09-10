using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Services
{
    public interface ITokenGenerator
    {
        string GenerateToken(SecurityTokenDescriptor tokenDescriptor);
    }
}
