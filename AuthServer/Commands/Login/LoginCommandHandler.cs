using AuthServer.Authorization.Descriptors.Token;
using AuthServer.Data.Tokens;
using AuthServer.Models;
using AuthServer.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokensDbContext _tokenDbContext;

        public LoginCommandHandler(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IConfiguration configuration,
            ITokenGenerator tokenGenerator, ITokensDbContext tokenDbContext) =>
            (_userManager, _signInManager, _configuration, _tokenGenerator, _tokenDbContext) =
            (userManager, signInManager, configuration, tokenGenerator, tokenDbContext);

        public async Task<string> Handle(LoginCommand request, 
            CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new AuthException("Пользователь не найден");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user, request.Password, false);

            if (!result.Succeeded)
            {
                throw new AuthException("Пароль не верный");
            }

            string token = _tokenGenerator.GenerateToken(
                new UserTokenDescriptor(user, _configuration));

            var accessToken = new AccessToken
            {
                UserId = user.Id,
                Value = token,
            };

            await _tokenDbContext.Tokens.AddAsync(accessToken);
            await _tokenDbContext.SaveChangesAsync(cancellationToken);

            return token;
        }
    }
}
