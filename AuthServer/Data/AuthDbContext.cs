using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthServer.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Data
{
    public class AuthDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));
            builder.Entity<IdentityRole>(entity => 
                entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<Guid>>(entity =>
                entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserLogin<Guid>>(entity =>
                entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityUserToken<Guid>>(entity =>
                entity.ToTable(name: "UserTokens"));
            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
                entity.ToTable(name: "RoleClaims"));

            builder.ApplyConfiguration(new AppUserConfiguration());
        }
    }
}
