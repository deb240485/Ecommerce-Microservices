using Auth.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.RefreshToken).HasMaxLength(500);
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(250);
            });

            // Seed default roles
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator role with full access"
                },
                new ApplicationRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "Standard user role"
                }
            );
        }
    }
}
