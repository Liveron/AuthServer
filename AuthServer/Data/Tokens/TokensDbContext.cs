using AuthServer.Data.Tokens.EntityTypeConfiguration;
using AuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data.Tokens
{
    public class TokensDbContext : DbContext, ITokensDbContext
    {
        public DbSet<AccessToken> Tokens { get; set; }

        public TokensDbContext(DbContextOptions<TokensDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TokenConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
