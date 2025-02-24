using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RunClubAPI.Models
{
    public class RunClubContext : IdentityDbContext<IdentityUser>
    {
        public RunClubContext(DbContextOptions<RunClubContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ProgressRecord> ProgressRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity tables are correctly created

            // Manually configure the Identity entity primary keys if necessary
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(u => new { u.UserId, u.LoginProvider, u.ProviderKey });

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(u => new { u.UserId, u.RoleId });

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(u => new { u.UserId, u.LoginProvider, u.Name });
            
            // Seeding roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Coach", NormalizedName = "COACH" },
                new IdentityRole { Id = "3", Name = "Runner", NormalizedName = "RUNNER" }
            );
        }
    }

}
