using AuthServer.Data.Tokens;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Commands.LogOut
{
    public class LogOutCommandHandler : IRequestHandler<LogOutCommand>
    {
        private readonly ITokensDbContext _tokensDbContext;

        public LogOutCommandHandler(ITokensDbContext tokensDbContext) =>
            _tokensDbContext = tokensDbContext;

        public async Task<Unit> Handle(LogOutCommand request, 
            CancellationToken cancellationToken)
        {
            var token = await _tokensDbContext.Tokens.
                FirstOrDefaultAsync(t => t.UserId == request.Id, cancellationToken);
            
            if (token == null)
            {
                throw new AuthException("Вы не вошли в систему");
            }

            _tokensDbContext.Tokens.Remove(token);
            await _tokensDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
