using AuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data.Tokens
{
    public interface ITokensDbContext
    {
        DbSet<AccessToken> Tokens { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
