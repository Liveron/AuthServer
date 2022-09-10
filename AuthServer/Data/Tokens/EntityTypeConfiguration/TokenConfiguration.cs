using AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Data.Tokens.EntityTypeConfiguration
{
    public class TokenConfiguration : IEntityTypeConfiguration<AccessToken>
    {
        public void Configure(EntityTypeBuilder<AccessToken> builder)
        {
            builder.HasKey(t => t.UserId);
        }
    }
}
