using AuthServer.Authorization.Descriptors.Token;
using AuthServer.Data.Tokens;
using AuthServer.Models;
using AuthServer.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Commands.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, string>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokensDbContext _tokensDbContext;

        public CreateUserCommandHandler(UserManager<AppUser> userManager,
            IConfiguration configuration, ITokenGenerator tokenGenerator,
            ITokensDbContext tokensDbContext) =>
            (_userManager, _configuration, _tokenGenerator, _tokensDbContext) =
            (userManager, configuration, tokenGenerator, tokensDbContext);

        public async Task<string> Handle(CreateUserCommand request, 
            CancellationToken cancellationToken)
        {
            var User = new AppUser
            {
                UserName = request.UserName,
                PasswordHash = request.Password,
            };

            var result = await _userManager.CreateAsync(User);

            string token = _tokenGenerator.GenerateToken(
                new UserTokenDescriptor(User, _configuration));

            if (!result.Succeeded)
            {
                throw new AuthException("Регистрация не удалась");
            }

            var accToken = new AccessToken
            {
                UserId = User.Id,
                Value = token,
            };

            await _tokensDbContext.Tokens.AddAsync(accToken, cancellationToken);
            await _tokensDbContext.SaveChangesAsync(cancellationToken);

            return token;
        }
    }
}
